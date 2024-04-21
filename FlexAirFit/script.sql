-- Database: FlexAirFitDB

-- DROP DATABASE IF EXISTS "FlexAirFitDB";

-- CREATE DATABASE "FlexAirFitDB"
--     WITH
--     OWNER = postgres
--     ENCODING = 'UTF8'
--     LC_COLLATE = 'Russian_Russia.1251'
--     LC_CTYPE = 'Russian_Russia.1251'
--     LOCALE_PROVIDER = 'libc'
--     TABLESPACE = pg_default
--     CONNECTION LIMIT = -1
--     IS_TEMPLATE = False;

CREATE TABLE IF NOT EXISTS "Users" (
    id UUID PRIMARY KEY,
    role INT NOT NULL,
    email VARCHAR(100) NOT NULL,
    password VARCHAR(100) NOT NULL
);

CREATE TABLE IF NOT EXISTS "Trainers" (
    id UUID PRIMARY KEY NOT NULL REFERENCES "Users" (id),
    name VARCHAR(50) NOT NULL,
    gender VARCHAR(10) NOT NULL CHECK (gender IN ('male', 'female')),
    specialization VARCHAR(50) NOT NULL,
    experience INT NOT NULL,
    rating INT NOT NULL CHECK (rating >= 1 AND rating <= 5)
);

CREATE TABLE IF NOT EXISTS "Admins" (
    id UUID NOT NULL REFERENCES "Users" (id),
    name VARCHAR(50) NOT NULL,
	date_of_birth DATE NOT NULL, 
    gender VARCHAR(10) NOT NULL CHECK (gender IN ('male', 'female'))
);

CREATE TABLE IF NOT EXISTS "Workouts" (
    id UUID PRIMARY KEY,
    name VARCHAR(50) NOT NULL,
    description VARCHAR(200) NOT NULL,
    trainer_id UUID NOT NULL REFERENCES "Trainers" (id),
    duration INTERVAL NOT NULL,
    level INT NOT NULL CHECK (level >= 1 AND level <= 5)
);

CREATE TABLE IF NOT EXISTS "Memberships" (
    id UUID PRIMARY KEY,
    name VARCHAR(50) NOT NULL,
    duration INTERVAL NOT NULL,
    price INT NOT NULL,
    freezing INT NOT NULL
);
CREATE TABLE IF NOT EXISTS "Clients" (
    id UUID PRIMARY KEY NOT NULL REFERENCES "Users" (id),
    name VARCHAR(50) NOT NULL,
    gender VARCHAR(10) NOT NULL CHECK (gender IN ('male', 'female')),
    date_of_birth DATE NOT NULL,
    membership_id UUID NOT NULL REFERENCES "Memberships" (id),
    membership_end DATE NOT NULL,
    remain_freezing INT
);

CREATE TABLE IF NOT EXISTS "Schedules" (
    id UUID PRIMARY KEY,
    workout_id UUID NOT NULL REFERENCES "Workouts" (id),
    date_and_time TIMESTAMP NOT NULL,
    client_id UUID REFERENCES "Clients" (id)
);

CREATE TABLE IF NOT EXISTS "Products" (
    id UUID PRIMARY KEY,
    type VARCHAR(50) NOT NULL,
    name VARCHAR(50) NOT NULL,
    price INT NOT NULL
);

CREATE TABLE IF NOT EXISTS "ClientProducts" (
    id_client UUID NOT NULL REFERENCES "Clients" (id),
    id_product UUID NOT NULL REFERENCES "Products" (id)
);

CREATE TABLE IF NOT EXISTS "Bonuses" (
	id UUID PRIMARY KEY,
	client_id UUID NOT NULL REFERENCES "Clients" (id),
	count INT NOT NULL
);

-- Добавление записи о тренере
-- INSERT INTO "Trainers" (id, user_id, name, gender, specialization, experience, rating)
-- VALUES ('123e4567-e89b-12d3-a456-426614174001', 'a958c596-d20b-4462-8064-ab5f192e3402', 'John Doe', 'male', 'Fitness', 5, 4);

-- -- Добавление записи о тренировке
-- INSERT INTO "Workouts" (id, name, description, trainer_id, duration, level)
-- VALUES ('223e4567-e89b-12d3-a456-426614174002', 'Cardio Blast', 'High-intensity cardio workout', '123e4567-e89b-12d3-a456-426614174001', '1 hour', 4);

-- -- Добавление записи о абонементе
-- INSERT INTO "Memberships" (id, name, duration, price, freezing)
-- VALUES ('423e4567-e89b-12d3-a456-426614174004', 'Standard', '365 days', 100, 30);

-- -- Добавление записи о клиенте
-- INSERT INTO "Clients" (id, user_id, name, gender, date_of_birth, membership_id, membership_end, remain_freezing, is_freezing)
-- VALUES ('323e4567-e89b-12d3-a456-426614174003', '017ed2b9-7db6-40c3-905c-103eabd18b1b', 'Alice Smith', 'female', '1990-05-15', '423e4567-e89b-12d3-a456-426614174004', '2024-05-15', NULL, 0);


