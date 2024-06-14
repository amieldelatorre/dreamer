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
-- Data for Name: Users; Type: TABLE DATA; Schema: public; Owner: root
--

INSERT INTO public."Users" VALUES ('c87407fe-4af5-494f-afd9-e987ffdaa92a', 'Isaac', 'Newton', 'isaac.newton@example.invalid', '$2a$11$hcC9vWCQwt1wVs75nJRTJ.V9d/GcYVr79DmAe.jA5PQRpdgjvWJte', '2024-06-07 22:31:19.004685+12', '2024-06-07 22:31:19.004945+12');
INSERT INTO public."Users" VALUES ('8b7fcf9d-1c48-4f5c-b153-095034661c5f', 'Albert', 'Einstein', 'albert.einstein@example.invalid', '$2a$11$opVz6EmbbCWiXt5rVOEs6.0l3vAKHeD3wNutHtXnuL2LLJGuKpyhu', '2024-06-09 10:48:39.284122+12', '2024-06-09 10:48:39.284235+12');


--
-- Name: Users PK_Users; Type: CONSTRAINT; Schema: public; Owner: root
--

ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT "PK_Users" PRIMARY KEY ("Id");


--
-- Name: IX_USER_EMAIL_UNIQUE; Type: INDEX; Schema: public; Owner: root
--

CREATE UNIQUE INDEX "IX_USER_EMAIL_UNIQUE" ON public."Users" USING btree ("Email");


--
-- PostgreSQL database dump complete
--

