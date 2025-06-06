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

CREATE TABLE public.users (
    userid integer GENERATED ALWAYS AS IDENTITY,
    name character varying(100) NOT NULL,
    email character varying(100) NOT NULL,
    password text NOT NULL,
    role character varying(10) NOT NULL,
    created_at date NOT NULL,
    CONSTRAINT users_pkey PRIMARY KEY (userid)
);

ALTER TABLE public.users OWNER TO postgres;

CREATE TABLE public.tasks (
    id integer GENERATED ALWAYS AS IDENTITY,
    parent_id integer NOT NULL,
    child_id integer NOT NULL,
    description text NOT NULL,
    deadline date,
    reward text,
    status character varying(20) NOT NULL DEFAULT 'ongoing',
    created_at date NOT NULL DEFAULT CURRENT_DATE,
    
    CONSTRAINT tasks_pkey PRIMARY KEY (id),
    CONSTRAINT task_parent_fk FOREIGN KEY (parent_id)
      REFERENCES public.users(userid) ON DELETE RESTRICT,
    CONSTRAINT task_child_fk FOREIGN KEY (child_id)
      REFERENCES public.users(userid) ON DELETE RESTRICT
);

-- Create indexes for better performance
CREATE INDEX idx_tasks_parent ON public.tasks(parent_id);
CREATE INDEX idx_tasks_child ON public.tasks(child_id);
CREATE INDEX idx_tasks_status ON public.tasks(status);

-- Set table ownership
ALTER TABLE public.tasks OWNER TO postgres;

-- Add table comments
COMMENT ON TABLE public.users IS 'System users including parents and children';
COMMENT ON TABLE public.tasks IS 'Tasks assigned by parents to children';

--
-- Data for Name: tasks; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.tasks (id, parent_id, child_id, description, deadline, reward, status, created_at) FROM stdin;
\.

--
-- Data for Name: users; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.users (userid, name, email, password, role, created_at) FROM stdin;
\.

--
-- Name: tasks childid; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tasks
    ADD CONSTRAINT childid FOREIGN KEY (child_id) REFERENCES public.users(userid);

--
-- Name: tasks parentid; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tasks
    ADD CONSTRAINT parentid FOREIGN KEY (parent_id) REFERENCES public.users(userid);

-- Completed on 2025-05-09 20:47:19

--
-- PostgreSQL database dump complete
--