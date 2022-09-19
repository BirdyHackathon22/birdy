from birdyml.capture import Capture

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


def main():
    # Motion detect Loop
    capture = Capture(config)
    while True:
        save_dir = capture.capture_event()


main()
