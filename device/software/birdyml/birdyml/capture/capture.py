#!/usr/bin/env python3
import tempfile
from datetime import datetime
from pathlib import Path
from time import sleep

from gpiozero import MotionSensor
from picamera2 import Picamera2

config = {
    'sensor_config': {
        'pin': 4,
        'queue_len': 5,
        'sample_rate': 10,
        'threshold': 0.99,
    },
    'camera_config': {
        'tuning_file_path': 'imx219.json',
        'initial_delay': 0,
        'delay': 1,
        'num_files': 5,
        'show_preview': False,
    },
}


class Camera:
    def __init__(self, config):
        print("Initializing Camera...")
        tuning_settings = Picamera2.load_tuning_file(config['tuning_file_path'])
        self.device_camera = Picamera2(tuning=tuning_settings)
        camera_config = self.device_camera.create_still_configuration()
        self.device_camera.still_configuration.size = (1280, 1280)
        self.device_camera.configure(camera_config)
        self.config = config
        print("Camera Initialized")

    def capture(self) -> tempfile.TemporaryDirectory:
        now = datetime.utcnow().strftime('%Y%m%d_%H%M%S')
        print(f"{now} - PIR Activated!")
        # Taking set of pictures
        print(f"{now} - Taking photos!")
        save_dir = tempfile.TemporaryDirectory()
        image_name = Path(save_dir.name) / f"image-{now}""-{:d}.jpeg"
        self.device_camera.start_and_capture_files(
            str(image_name),
            initial_delay=self.config['initial_delay'],
            delay=self.config['delay'],
            num_files=self.config['num_files'],
            show_preview=self.config['show_preview'],
        )
        print(f"{now} - All Done!")
        return save_dir


class Capture:
    def __init__(self, config):
        print("Initializing Motion PIR...")
        self.pir = MotionSensor(**config['sensor_config'])
        print("Motion PIR initialized")
        self.camera = Camera(config['camera_config'])

    def capture_event(self) -> tempfile.TemporaryDirectory:
        self.pir.wait_for_active()
        while self.pir.is_active:
            save_dir = self.camera.capture()
            self.pir.wait_for_inactive()
            sleep(2)
        return save_dir


def main():
    # Motion detect Loop
    capture = Capture(config)
    while True:
        save_dir = capture.capture_event()


if __name__ == "__main__":
    main()
