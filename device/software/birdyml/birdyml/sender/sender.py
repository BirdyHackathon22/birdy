import datetime as dt
from dataclasses import dataclass

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
        pass

    def push_image(self, image):
        return image.stem

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
        print(obs)
