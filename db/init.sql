--
-- PostgreSQL database dump
--

-- Dumped from database version 17.4
-- Dumped by pg_dump version 17.4

-- Started on 2025-05-09 20:47:19

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
-- TOC entry 218 (class 1259 OID 16396)
-- Name: tasks; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tasks (
    id integer NOT NULL,
    parent_id integer NOT NULL,
    child_id integer NOT NULL,
    description text NOT NULL,
    deadline date,
    reward text,
    status character varying(20) NOT NULL,
    created_at date NOT NULL
);


ALTER TABLE public.tasks OWNER TO postgres;

--
-- TOC entry 217 (class 1259 OID 16389)
-- Name: users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.users (
    id integer NOT NULL,
    name character varying(100) NOT NULL,
    email character varying(100) NOT NULL,
    password text NOT NULL,
    role character varying(10) NOT NULL,
    created_at date NOT NULL
);


ALTER TABLE public.users OWNER TO postgres;

--
-- TOC entry 4897 (class 0 OID 16396)
-- Dependencies: 218
-- Data for Name: tasks; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.tasks (id, parent_id, child_id, description, deadline, reward, status, created_at) FROM stdin;
\.


--
-- TOC entry 4896 (class 0 OID 16389)
-- Dependencies: 217
-- Data for Name: users; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.users (id, name, email, password, role, created_at) FROM stdin;
\.


--
-- TOC entry 4748 (class 2606 OID 16402)
-- Name: tasks tasks_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tasks
    ADD CONSTRAINT tasks_pkey PRIMARY KEY (id);


--
-- TOC entry 4746 (class 2606 OID 16395)
-- Name: users users_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (id);


--
-- TOC entry 4749 (class 2606 OID 16403)
-- Name: tasks childid; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tasks
    ADD CONSTRAINT childid FOREIGN KEY (child_id) REFERENCES public.users(id);


--
-- TOC entry 4750 (class 2606 OID 16408)
-- Name: tasks parentid; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tasks
    ADD CONSTRAINT parentid FOREIGN KEY (parent_id) REFERENCES public.users(id);


-- Completed on 2025-05-09 20:47:19

--
-- PostgreSQL database dump complete
--

