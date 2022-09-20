import argparse
import json
from pathlib import Path

from birdyml.capture import Capture
from birdyml.classifier import BirdDetector


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument('config_file', type=Path)
    args = parser.parse_args()
    with open(args.config_file, 'r') as fp:
        config = json.load(fp)

    capture = Capture(config)
    detector = BirdDetector(config)

    while True:
        save_dir = capture.capture_event()
        images = Path(save_dir.name).iterdir()
        bird = detector.predict(images)
        print(bird)


main()
