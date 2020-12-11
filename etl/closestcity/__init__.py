import logging
from rich.logging import RichHandler
import os

FORMAT = "%(message)s"
LEVEL = os.environ.get('LOGGING_LEVEL', 'INFO')

logging.basicConfig(
    level=LEVEL, format=FORMAT, datefmt="[%X]", handlers=[RichHandler()]
)
