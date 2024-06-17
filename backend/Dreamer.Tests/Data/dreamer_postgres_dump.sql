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

INSERT INTO public."Users" VALUES ('68d1e183-330c-4349-869a-64679341b21e', 'Isaac', 'Newton', 'isaac.newton@example.invalid', '$2a$11$HG0gfVXyolIyKaHrSh39mObJlz.BP6.e/6kprDJ60icNAxKFQjgw.', '2024-06-17 18:54:35.985008+12', '2024-06-17 18:54:35.985139+12');
INSERT INTO public."Users" VALUES ('0684514e-54e7-4613-9b74-332890c7dca9', 'Albert', 'Einstein', 'albert.einstein@example.invalid', '$2a$11$2TA8.qbXSS8fsTfN/gR58es4wtof37BH/JWDPE5/Jcv3sgyUkHHdS', '2024-06-17 18:54:38.46539+12', '2024-06-17 18:54:38.465395+12');


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

