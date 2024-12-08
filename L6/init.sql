--
-- PostgreSQL database dump
--

-- Dumped from database version 17.0
-- Dumped by pg_dump version 17.0

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
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
-- Name: Comments; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Comments" (
    "Id" integer NOT NULL,
    "TicketId" integer NOT NULL,
    "Author" text NOT NULL,
    "Content" text NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL
);


ALTER TABLE public."Comments" OWNER TO postgres;

--
-- Name: Comments_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public."Comments" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."Comments_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: Tickets; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Tickets" (
    "Id" integer NOT NULL,
    "Title" text NOT NULL,
    "Description" text NOT NULL,
    "Status" text NOT NULL,
    "Priority" text NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL
);


ALTER TABLE public."Tickets" OWNER TO postgres;

--
-- Name: Tickets_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public."Tickets" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."Tickets_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO postgres;

--
-- Data for Name: Comments; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Comments" ("Id", "TicketId", "Author", "Content", "CreatedAt") FROM stdin;
\.


--
-- Data for Name: Tickets; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Tickets" ("Id", "Title", "Description", "Status", "Priority", "CreatedAt", "UpdatedAt") FROM stdin;
2	t	t	Open	Low	2024-12-07 15:09:57.348+01	2024-12-07 15:09:57.348+01
3	test	test	Open	Medium	2024-12-07 15:12:03+01	2024-12-07 15:12:03+01
4	1	1	Open	Medium	2024-12-07 15:14:03.155+01	2024-12-07 15:14:03.155+01
5	112312	123	Open	Medium	2024-12-07 15:22:56.899+01	2024-12-07 15:22:56.899+01
1	t1	t1	Otwarty	High	2024-12-07 15:46:53.214639+01	2024-12-07 15:46:53.214645+01
\.


--
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
20241205135929_Init	9.0.0
20241207142138_TimeZonesEdit	9.0.0
\.


--
-- Name: Comments_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."Comments_Id_seq"', 1, false);


--
-- Name: Tickets_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."Tickets_Id_seq"', 5, true);


--
-- Name: Comments PK_Comments; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Comments"
    ADD CONSTRAINT "PK_Comments" PRIMARY KEY ("Id");


--
-- Name: Tickets PK_Tickets; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Tickets"
    ADD CONSTRAINT "PK_Tickets" PRIMARY KEY ("Id");


--
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- Name: IX_Comments_TicketId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Comments_TicketId" ON public."Comments" USING btree ("TicketId");


--
-- Name: Comments FK_Comments_Tickets_TicketId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Comments"
    ADD CONSTRAINT "FK_Comments_Tickets_TicketId" FOREIGN KEY ("TicketId") REFERENCES public."Tickets"("Id") ON DELETE CASCADE;


--
-- PostgreSQL database dump complete
--

