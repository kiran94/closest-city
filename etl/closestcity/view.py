import sqlalchemy
from sqlalchemy.engine import Engine
import logging
import psycopg2

logger = logging.getLogger(__name__)


def create_view(**kwargs):

    worldcities_file = kwargs.get('worldcities_file')
    srid = kwargs.get('srid')
    publish = kwargs.get('publish')

    table = worldcities_file.split('.')[0]

    engine: Engine = sqlalchemy.create_engine(kwargs.get('write_url'))
    create_view = f'CREATE MATERIALIZED VIEW {table}_geometry AS select *, CAST(ST_SetSRID(ST_MakePoint(lng, lat), {srid}) as geography) as geom from {table}'

    logger.info(f'Applying {create_view}')

    if publish:

        try:
            engine.execute(create_view)
        except sqlalchemy.exc.ProgrammingError as e:
            if isinstance(e.orig, psycopg2.errors.DuplicateTable):
                logger.warning('View already existed, skipping')
            else:
                raise
    else:
        logger.warning('Publish flag was not set, skipping')