CREATE LOGIN root WITH PASSWORD = "root", CHECK_POLICY = OFF;
CREATE USER root FOR LOGIN root;
ALTER SERVER ROLE dbcreator ADD MEMBER root;
GO