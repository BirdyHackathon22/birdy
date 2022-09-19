import argparse
import json
from pathlib import Path

from birdyml.capture import Capture


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument('config_file', type=Path)
    args = parser.parse_args()
    with open(args.config_file, 'r') as fp:
        config = json.load(fp)

    capture = Capture(config)
    while True:
        save_dir = capture.capture_event()


main()
