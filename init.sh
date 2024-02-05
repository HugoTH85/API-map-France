#!/bin/bash
set -e

mysql --protocol=socket -uroot -proot --socket=/var/run/mysqld/mysqld.sock --binary-mode=1 projet_archi2 < /docker-entrypoint-initdb.d/db_carte_france.sql