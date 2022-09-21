import argparse
import json
from pathlib import Path

from birdyml.capture import Capture
from birdyml.classifier import BirdDetector, NotBirdError
from birdyml.sender import Sender


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument('config_file', type=Path)
    args = parser.parse_args()
    with open(args.config_file, 'r') as fp:
        config = json.load(fp)

    capture = Capture(config)
    detector = BirdDetector(config)
    sender = Sender(config)

    while True:
        save_dir = capture.capture_event()
        images = Path(save_dir.name).iterdir()
        try:
            bird = detector.predict(images)
        except NotBirdError:
            print('No bird found in images')
            continue
        sender.push_bird(bird)


main()
