import os
from dataclasses import dataclass


@dataclass
class Location:
    latitude: str
    longitude: str


class Birdy:
    def __init__(self):
        self.device = os.getenv('BIRDY_DEVICE', 'birdy001')
        self.location = self._get_location()

    def _get_location(self) -> Location:
        return Location('47.45', '8.56')


birdy = Birdy()
