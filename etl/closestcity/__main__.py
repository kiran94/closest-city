import argparse
import logging
import os
from closestcity.download import download, unzip
from closestcity.load import load
from closestcity.view import create_view

logger = logging.getLogger(__name__)

if __name__ == "__main__":

    parser = argparse.ArgumentParser()
    parser.add_argument('-s', '--step', choices=['download', 'unzip', 'load', 'create_view'], required=True)
    parser.add_argument('-u', '--url', default='https://simplemaps.com/static/data/world-cities/basic/simplemaps_worldcities_basicv1.73.zip')
    parser.add_argument('-w', '--working_dir', default='data')
    parser.add_argument('--worldcities_file_zip', default='worldcities.zip')
    parser.add_argument('--worldcities_file', default='worldcities.csv')
    parser.add_argument(
        '--write_url', 
        default=f'postgres://{os.environ["PGUSER"]}:{os.environ["PGPASSWORD"]}@{os.environ.get("PGHOST", "localhost")}:{os.environ.get("PGPORT", "5432")}/{os.environ.get("PGDATABASE", "closestcity")}')
    parser.add_argument('--publish', action='store_true')
    parser.add_argument('--srid', default='4326')

    args = parser.parse_args()
    logger.debug(vars(args))

    logger.debug('Ensuring %s exists', args.working_dir)
    os.makedirs(args.working_dir, exist_ok=True)

    if args.step == 'download':
        download(**(vars(args)))
    elif args.step == 'unzip':
        unzip(**(vars(args)))
    elif args.step == 'load':
        load(**(vars(args)))
    elif args.step == 'create_view':
        create_view(**(vars(args)))

    logger.info('Done')
