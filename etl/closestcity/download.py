import logging
import requests
import os
import zipfile

logger = logging.getLogger(__name__)


def download(**kwargs) -> str:
    '''
    Downloads the World Cities file to the working directory
    '''
    url = kwargs.get('url')
    working_dir = kwargs.get('working_dir')
    worldcities_file_zip = kwargs.get('worldcities_file_zip')

    target_file = os.path.join(working_dir, worldcities_file_zip)

    logger.info('Downloading %s -> %s', url, target_file)
    response = requests.get(url)
    logger.debug(response.status_code)

    with open(target_file, 'wb') as f:
        f.write(response.content)

    return target_file


def unzip(**kwargs) -> str:
    '''
    Unzips the contents of the zip file passed into the working directory
    '''
    working_dir = kwargs.get('working_dir')
    worldcities_file_zip = kwargs.get('worldcities_file_zip')
    worldcities_file = kwargs.get('worldcities_file')

    source_file = os.path.join(working_dir, worldcities_file_zip)
    target_file = os.path.join(working_dir, worldcities_file)

    logger.info('Unzipping %s -> %s', source_file, target_file)

    with zipfile.ZipFile(source_file, 'r') as zip:

        files = zip.namelist()
        logger.info('Found %s', files)

        to_extract = [f for f in files if f == os.path.basename(target_file)]

        try:
            to_extract = to_extract[0]
        except IndexError:
            logger.exception(f'{os.path.basename(target_file)} not found')

        logger.info('Writing %s', target_file)
        with open(target_file, 'wb') as f:
            f.write(zip.read(to_extract))
