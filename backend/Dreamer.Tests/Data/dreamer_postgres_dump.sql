--
-- PostgreSQL database dump
--

-- Dumped from database version 16.1 (Debian 16.1-1.pgdg120+1)
-- Dumped by pg_dump version 16.1 (Debian 16.1-1.pgdg120+1)

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: Jwts; Type: TABLE; Schema: public; Owner: root
--

CREATE TABLE public."Jwts" (
    "Id" uuid NOT NULL,
    "ExpiryDate" timestamp with time zone NOT NULL,
    "DateCreated" timestamp with time zone NOT NULL,
    "DateModified" timestamp with time zone NOT NULL,
    "IsDisabled" boolean NOT NULL,
    "DateDisabled" timestamp with time zone,
    "UserId" uuid NOT NULL
);


ALTER TABLE public."Jwts" OWNER TO root;

--
-- Name: Users; Type: TABLE; Schema: public; Owner: root
--

CREATE TABLE public."Users" (
    "Id" uuid NOT NULL,
    "FirstName" text NOT NULL,
    "LastName" text NOT NULL,
    "Email" text NOT NULL,
    "Password" text NOT NULL,
    "DateCreated" timestamp with time zone NOT NULL,
    "DateModified" timestamp with time zone NOT NULL
);


ALTER TABLE public."Users" OWNER TO root;

--
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: root
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO root;

--
-- Data for Name: Jwts; Type: TABLE DATA; Schema: public; Owner: root
--



--
-- Data for Name: Users; Type: TABLE DATA; Schema: public; Owner: root
--

INSERT INTO public."Users" VALUES ('8fb15cbf-5d6d-4de2-a640-7c5d4310131d', 'Isaac', 'Newton', 'isaac.newton@example.invalid', '$2a$11$XugBDskCU37eD99lFgLP6uooptKXdqMJPrxGUXcO2y64/DrHURiMW', '2024-06-17 18:43:24.167594+12', '2024-06-17 18:43:24.167976+12');
INSERT INTO public."Users" VALUES ('04156bbf-4e61-412c-9d72-c8d92699c5a7', 'Albert', 'Einstein', 'albert.einstein@example.invalid', '$2a$11$.4VR8Qi1yNZ/6jOMkTIDneV3vq1d0AiHTefMKVIyRYnP8aixH9l02', '2024-06-17 18:43:28.069013+12', '2024-06-17 18:43:28.069018+12');


--
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: root
--

INSERT INTO public."__EFMigrationsHistory" VALUES ('20240617063152_InitialMigration', '8.0.6');


--
-- Name: Jwts PK_Jwts; Type: CONSTRAINT; Schema: public; Owner: root
--

ALTER TABLE ONLY public."Jwts"
    ADD CONSTRAINT "PK_Jwts" PRIMARY KEY ("Id");


--
-- Name: Users PK_Users; Type: CONSTRAINT; Schema: public; Owner: root
--

ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT "PK_Users" PRIMARY KEY ("Id");


--
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: root
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- Name: IX_Jwts_UserId; Type: INDEX; Schema: public; Owner: root
--

CREATE INDEX "IX_Jwts_UserId" ON public."Jwts" USING btree ("UserId");


--
-- Name: IX_USER_EMAIL_UNIQUE; Type: INDEX; Schema: public; Owner: root
--

CREATE UNIQUE INDEX "IX_USER_EMAIL_UNIQUE" ON public."Users" USING btree ("Email");


--
-- Name: Jwts FK_Jwts_Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: root
--

ALTER TABLE ONLY public."Jwts"
    ADD CONSTRAINT "FK_Jwts_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE CASCADE;


--
-- PostgreSQL database dump complete
--

