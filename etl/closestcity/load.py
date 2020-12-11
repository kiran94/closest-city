import logging
import os

from closestcity import log_dataframe

import pandas as pd

logger = logging.getLogger(__name__)


def load(**kwargs):
    '''
    Loads the provided file into a Database
    '''
    working_dir = kwargs.get('working_dir')
    worldcities_file = kwargs.get('worldcities_file')

    source_file = os.path.join(working_dir, worldcities_file)
    print(source_file)

    logging.info('Loading %s', source_file)
    frame: pd.DataFrame = pd.read_csv(source_file, index_col=['id'])

    logger.info('Loaded %s rows, %s columns', *frame.shape)
    log_dataframe(frame, logger)

