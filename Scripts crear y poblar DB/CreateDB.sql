
CREATE DATABASE MiBaseDeDatos
ON
(
    NAME = 'GUARDIANS_OG_Data',
    FILENAME = 'C:\DB\GUARDIANS_OG_Data.mdf',
    SIZE = 10MB,  -- Tamaño inicial del archivo de datos
    MAXSIZE = UNLIMITED,  -- Tamaño máximo del archivo de datos
    FILEGROWTH = 5MB  -- Incremento automático del archivo de datos
)
LOG ON
(
    NAME = 'GUARDIANS_OG_Log',
    FILENAME = 'C:\DB\GUARDIANS_OG.ldf',
    SIZE = 5MB,  -- Tamaño inicial del archivo de registro de transacciones
    MAXSIZE = 1GB,  -- Tamaño máximo del archivo de registro de transacciones
    FILEGROWTH = 1MB  -- Incremento automático del archivo de registro de transacciones
);

CREATE TABLE users (username varchar(20) PRIMARY KEY, pw varchar(30), img image);

CREATE TABLE  hero (hero_ID int IDENTITY(1,1) PRIMARY KEY, img image, hero_name varchar(50), real_name varchar(100), powers varchar(100), 
weaks varchar(100), relations varchar(200), age int, cellphone varchar(20), origin varchar(100));

CREATE TABLE  villain (villain_ID int IDENTITY(1,1) PRIMARY KEY, img image, villain_name varchar(50), real_name varchar(100), powers varchar(100), 
weaks varchar(100), relations varchar(200), age int, cellphone varchar(20), origin varchar(100));

CREATE TABLE task (task_ID int PRIMARY KEY, task_name varchar(50), hero_ID INT NOT NULL, sdate DATETIME, fdate DATETIME, task_status int, edited_by varchar(20),edited_at DATE, 
FOREIGN KEY(hero_ID) REFERENCES hero(hero_ID), FOREIGN KEY (edited_by) REFERENCES users(username) );

CREATE TABLE fight(fight_ID int IDENTITY(1,10) PRIMARY KEY, result int, hero_ID INT, villain_ID int, 
FOREIGN KEY(hero_ID) REFERENCES hero(hero_ID), FOREIGN KEY(villain_ID) REFERENCES villain(villain_ID), comments varchar(100) );

CREATE TABLE sponsor(trans_ID int IDENTITY(1,1) PRIMARY KEY,sponsor_ID  int, amount money, hero_ID INT, sourceOM varchar(100), 
FOREIGN KEY(hero_ID) REFERENCES hero(hero_ID));

CREATE TABLE history_user(username varchar(20) PRIMARY KEY, recent_searchs varchar(200),
FOREIGN KEY(username) REFERENCES users(username));

CREATE SEQUENCE task_seq AS INT 
START WITH 0 
INCREMENT BY 1
MINVALUE 0
NO MAXVALUE;