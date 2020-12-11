import argparse
import logging
import os
from closestcity.download import download, unzip

logger = logging.getLogger(__name__)

if __name__ == "__main__":

    parser = argparse.ArgumentParser()
    parser.add_argument('-s', '--step', choices=['download', 'unzip'], required=True)
    parser.add_argument('-u', '--url', default='https://simplemaps.com/static/data/world-cities/basic/simplemaps_worldcities_basicv1.73.zip')
    parser.add_argument('-w', '--working_dir', default='data')
    parser.add_argument('--worldcities_file_zip', default='worldcities.zip')
    parser.add_argument('--worldcities_file', default='worldcities.csv')

    args = parser.parse_args()
    logger.debug(vars(args))

    logger.debug('Ensuring %s exists', args.working_dir)
    os.makedirs(args.working_dir, exist_ok=True)

    if args.step == 'download':
        download(**(vars(args)))
    elif args.step == 'unzip':
        unzip(**(vars(args)))

    logger.info('Done')
