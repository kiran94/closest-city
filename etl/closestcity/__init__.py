import logging
from rich.logging import RichHandler
import os
import pandas as pd

FORMAT = "%(message)s"
LEVEL = os.environ.get('LOGGING_LEVEL', 'INFO')

logging.basicConfig(
    level=LEVEL, format=FORMAT, datefmt="[%X]", handlers=[RichHandler()]
)


def log_dataframe(frame: pd.DataFrame, logger: logging.Logger, level: int = logging.DEBUG):
    if logger.isEnabledFor(level):
        print('\n', frame)
