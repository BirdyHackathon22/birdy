from dataclasses import dataclass
from typing import Any

@dataclass
class Bird:
    label : str
    score : int
    path : str
    box : Any
    location : str
    time : str
    device : str
