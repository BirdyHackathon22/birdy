from bird_detector import BirdDetector
import os
import sys

listOfPaths = ["test/bird.jpg"]

config = {
    "detector_config": {
        "modelLabels" :  os.path.join(sys.path[0], "birdy_efficientdet_lite2_index.txt"),
        "modelTensorflow" : os.path.join(sys.path[0], "birdy_efficientdet_lite2.tflite"),
        "probabilityThreshold" : 0.7
    },
    "classifier_config": {
        "modelLabels" :  os.path.join(sys.path[0], "birdy_aiyvision_index.txt"),
        "modelTensorflow" : os.path.join(sys.path[0], "birdy_aiyvision.tflite"),
        "probabilityThreshold" : 0.6
    }
}

bird_dc = BirdDetector(config)
print(bird_dc.predict(listOfPaths))
