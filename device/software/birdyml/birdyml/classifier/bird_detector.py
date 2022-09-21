from dataclasses import dataclass

import numpy as np
from PIL import Image, ImageDraw
from tflite_runtime.interpreter import Interpreter


class NotBirdError(Exception):
    pass


@dataclass
class Point:
    x: float
    y: float

    def __str__(self):
        return f"[{self.x}, {self.y}]"


@dataclass
class Box:
    min_point: Point
    max_point: Point

    @classmethod
    def from_list(cls, box_list):
        return cls(*box_list)

    def __str__(self):
        return f"[{self.min_point}, {self.max_point}]"


@dataclass
class Bird:
    label: str
    score_label: float
    score: float
    path: str
    box: Box


class BirdDetector:

    def __init__(self, config):
        self.detector = Detector(config["detector_config"])
        self.classifier = Classifier(config["classifier_config"])

    def predict(self, listOfImages):
        detector_result = self.detector.isBird(listOfImages)
        if not detector_result['is_bird']:
            raise NotBirdError('No bird found')

        classifier_result = self.classifier.identifyBird(
            detector_result['path'],
            detector_result['box'],
        )
        return Bird(
            classifier_result['label'],
            classifier_result['score'],
            detector_result['score'],
            detector_result['path'],
            Box.from_list(detector_result['box']),
        )


class Detector:
    def __init__(self, config):
        # config object
        self.modelLabels = config["modelLabels"]
        self.probabilityThreshold = config["probabilityThreshold"]
        self.interpreter = Interpreter(config["modelTensorflow"])
        self.interpreter.allocate_tensors()
        self.input_details = self.interpreter.get_input_details()
        self.output_details = self.interpreter.get_output_details()
        _, self.input_height, self.input_width, _ = self.interpreter.get_input_details()[0]['shape']
        self.labels = self.loadModellabels()

    def loadModellabels(self):
        with open(self.modelLabels, 'r') as f:
            return {i: line.strip() for i, line in enumerate(f.readlines())}

    def isBird(self, listOfPaths) -> bool:
        self.score = 0
        self.label = None
        self.path = None
        self.box = None

        for path in listOfPaths:
            self.runDetection(path)

        return {
            "is_bird": self.score >= self.probabilityThreshold,
            "box": self.box,
            "path": self.path,
            "score": self.score,
        }

    def runDetection(self,path):
        # TensorFlow Model works with 448x448 image size, resizing the original image to match.
        image = Image.open(path)
        image_height, image_width = image.size[:2]
        resizedimage=image.resize((self.input_width, self.input_height))

        input_data = np.expand_dims(resizedimage, axis=0)
        self.interpreter.set_tensor(self.input_details[0]['index'], input_data)
        self.interpreter.invoke()

        # Retrieve detection results
        boxes = self.interpreter.get_tensor(self.output_details[0]['index'])[0]
        classes = self.interpreter.get_tensor(self.output_details[1]['index'])[0]
        scores = self.interpreter.get_tensor(self.output_details[2]['index'])[0]
        results= zip(scores, boxes, classes)

        with Image.open(path) as im:
            draw = ImageDraw.Draw(im)

        for score, box, objDetected in results:

            if score < self.probabilityThreshold:
                continue

            # filter the list to have birds, and choose the bird with the highest score

            min_y = round(box[0] * image_height)
            min_x = round(box[1] * image_width)
            max_y = round(box[2] * image_height)
            max_x = round(box[3] * image_width)

            if score*100 > self.score and self.labels[int(objDetected)] == 'bird':
                self.score = score*100
                self.path = path
                self.label = self.labels[int(objDetected)]
                self.box = [[min_x, min_y], [max_x, max_y]]

            print (f"{self.labels[int(objDetected)]}: {score*100:.2f}% - {min_x}:{min_y} - {max_x}:{max_y}")
            draw.rectangle([min_x,min_y,max_x,max_y], None, 128, 2)

class Classifier():
    def __init__(self, config):

        self.modelLabels = config["modelLabels"]
        self.labels = self.loadModellabels()
        self.interpreter = Interpreter(config["modelTensorflow"])
        self.interpreter.allocate_tensors()
        _, self.height, self.width, _ = self.interpreter.get_input_details()[0]['shape']

    def loadModellabels(self):
        # Load labels for the model
        with open(self.modelLabels, 'r') as f:
            return {i: line.strip() for i, line in enumerate(f.readlines())}

    def identifyBird(self, path, box) -> Bird :
        """ is there a bird at the feeder? """
        image = Image.open(path)
        # need to resize the image here so it becomes a square image as big as possible inside of the square defined by the two boxes
        max_length = max(box[1][0] - box[0][0], box[1][1] - box[0][1])/2
        center = ((box[1][0] + box[0][0])/2, (box[1][1] + box[0][1])/2)
        print('center :', center)
        print('max_length :', max_length)
        # consider edge cases when it could actually crash
        image.show()
        image = image.crop((center[0] - max_length, center[1] - max_length, center[0] + max_length, center[1] + max_length))
        image.show()

        resizedImage=image.resize((224,224))
        results = self.classifyImage(resizedImage)
        label_id, prob = results[0]
        print("bird: " + self.labels[label_id])
        print("prob: " + str(prob))

        bird = self.labels[label_id]
        bird = bird[bird.find(",") + 1:]
        prob_pct = str(round(prob * 100, 1)) + "%"
        return {
            "label": bird,
            "score": prob*100,
            "path": path,
            "box": box
        }

    def classifyImage(self, image, top_k=1):
        self.set_input_tensor(image)
        self.interpreter.invoke()
        output_details = self.interpreter.get_output_details()[0]
        output = np.squeeze(self.interpreter.get_tensor(output_details['index']))

        # if model is quantized (uint8 data), then dequantize the results
        if output_details['dtype'] == np.uint8:
            scale, zero_point = output_details['quantization']
            output = scale * (output - zero_point)

        ordered = np.argpartition(-output, top_k)
        return [(i, output[i]) for i in ordered[:top_k]]

    def set_input_tensor(self, image):
        tensor_index = self.interpreter.get_input_details()[0]['index']
        input_tensor = self.interpreter.tensor(tensor_index)()[0]
        input_tensor[:, :] = image
