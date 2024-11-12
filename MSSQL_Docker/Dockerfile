   FROM mcr.microsoft.com/mssql/server:2022-latest
ENV ACCEPT_EULA=Y
ENV MSSQL_SA_PASSWORD=hb8Aoq5ToKM3/wV4N9GZbR7VKC2Gh2NTkHlmYrCACR8

COPY script.sql /usr/config/script.sql
RUN /opt/mssql/bin/sqlservr & sleep 20 && /opt/mssql-tools18/bin/sqlcmd -s localhost -U sa -P ${MSSQL_SA_PASSWORD} -i /usr/config/script.sql -C

HEALTHCHECK --interval=10s --timeout=3s --start-interval=10s --retries=10 \
   CMD /opt/mssql-tools18/bin/sqlcmd -s localhost -U sa -P ${MSSQL_SA_PASSWORD} -Q "SELECT 1" -C || exit 1