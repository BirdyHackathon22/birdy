import json
import datetime as dt
from dataclasses import dataclass, asdict

import requests

from birdyml.birdy import birdy


@dataclass
class BirdObservation:
    label: str
    score: float
    path: str
    box: str
    latitude: str
    longitude: str
    time: str
    device: str


class Sender:
    def __init__(self, config):
        self.base_url = config['api_config']['base_url']

    def push_image(self, image):
        image_name = f"{birdy.device}-{image.name}"
        files = {'fileData': open(image, 'rb')}
        url = self.base_url + "/Media"
        response = requests.post(url, files=files)
        if not response.status_code == 200:
            raise RuntimeError('Pushing to API failed...')
        print(f'Pushed {response.text} to the API')
        return response.text

    def push_bird(self, bird):
        remote_path = self.push_image(bird.path)
        obs = BirdObservation(
            label=bird.label,
            score=bird.score,
            path=remote_path,
            box=str(bird.box),
            latitude=birdy.location.latitude,
            longitude=birdy.location.longitude,
            time=dt.datetime.utcnow().isoformat(),
            device=birdy.device,
        )
        headers = {
            'Content-type': 'application/json',
            'Accept': 'application/json'
        }
        url = self.base_url + '/BirdyWatch'
        response = requests.post(url, headers=headers, data=json.dumps(asdict(obs)))

        if not response.status_code == 200:
            print(f'Pushing bird observation failed')

        print(f'Successfully pushed {bird.label} to the API')
