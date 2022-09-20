from bird_detector import BirdDetector
import os
import sys

listOfPaths = ["test/house_sparrow_2.jpg", "test/house_sparrow_3.jpg", "test/house_sparrow_4.jpg"]

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

# cut the image according to min max coordinates
# what is the minimum square from the two points to that specific square
# adapting to the smallest size and centered in the middle
# create a square for the image