
SET default_transaction_read_only = off;

SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;

CREATE ROLE anon;
ALTER ROLE anon WITH NOSUPERUSER NOINHERIT NOCREATEROLE NOCREATEDB NOLOGIN NOREPLICATION NOBYPASSRLS;
CREATE ROLE authenticated;
ALTER ROLE authenticated WITH NOSUPERUSER NOINHERIT NOCREATEROLE NOCREATEDB NOLOGIN NOREPLICATION NOBYPASSRLS;
CREATE ROLE authenticator;
ALTER ROLE authenticator WITH NOSUPERUSER NOINHERIT NOCREATEROLE NOCREATEDB LOGIN NOREPLICATION NOBYPASSRLS PASSWORD '123';
CREATE ROLE dashboard_user;
ALTER ROLE dashboard_user WITH NOSUPERUSER INHERIT CREATEROLE CREATEDB NOLOGIN REPLICATION NOBYPASSRLS;
CREATE ROLE pgbouncer;
ALTER ROLE pgbouncer WITH NOSUPERUSER INHERIT NOCREATEROLE NOCREATEDB LOGIN NOREPLICATION NOBYPASSRLS PASSWORD '123';
CREATE ROLE postgres;
ALTER ROLE postgres WITH NOSUPERUSER INHERIT CREATEROLE CREATEDB LOGIN REPLICATION BYPASSRLS PASSWORD '123';
CREATE ROLE service_role;
ALTER ROLE service_role WITH NOSUPERUSER NOINHERIT NOCREATEROLE NOCREATEDB NOLOGIN NOREPLICATION BYPASSRLS;
CREATE ROLE supabase_admin;
ALTER ROLE supabase_admin WITH SUPERUSER INHERIT CREATEROLE CREATEDB LOGIN REPLICATION BYPASSRLS PASSWORD '123';
CREATE ROLE supabase_auth_admin;
ALTER ROLE supabase_auth_admin WITH NOSUPERUSER NOINHERIT CREATEROLE NOCREATEDB LOGIN NOREPLICATION NOBYPASSRLS PASSWORD '123';
CREATE ROLE supabase_storage_admin;
ALTER ROLE supabase_storage_admin WITH NOSUPERUSER NOINHERIT CREATEROLE NOCREATEDB LOGIN NOREPLICATION NOBYPASSRLS PASSWORD '123';

ALTER ROLE postgres SET search_path TO E'\\$user', 'public', 'extensions';

ALTER ROLE supabase_admin SET search_path TO '$user', 'public', 'auth', 'extensions';

ALTER ROLE supabase_auth_admin SET search_path TO 'auth';

ALTER ROLE supabase_storage_admin SET search_path TO 'storage';

GRANT anon TO authenticator GRANTED BY postgres;
GRANT authenticated TO authenticator GRANTED BY postgres;
GRANT service_role TO authenticator GRANTED BY postgres;
GRANT supabase_admin TO authenticator GRANTED BY postgres;

\connect template1

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

\connect postgres

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

CREATE SCHEMA auth;

ALTER SCHEMA auth OWNER TO supabase_admin;

CREATE SCHEMA extensions;

ALTER SCHEMA extensions OWNER TO postgres;

CREATE SCHEMA pgbouncer;

ALTER SCHEMA pgbouncer OWNER TO pgbouncer;

CREATE SCHEMA storage;

ALTER SCHEMA storage OWNER TO supabase_admin;

CREATE EXTENSION IF NOT EXISTS pg_stat_statements WITH SCHEMA extensions;

COMMENT ON EXTENSION pg_stat_statements IS 'track execution statistics of all SQL statements executed';

CREATE EXTENSION IF NOT EXISTS pgcrypto WITH SCHEMA extensions;

COMMENT ON EXTENSION pgcrypto IS 'cryptographic functions';

CREATE EXTENSION IF NOT EXISTS pgjwt WITH SCHEMA extensions;

COMMENT ON EXTENSION pgjwt IS 'JSON Web Token API for Postgresql';

CREATE EXTENSION IF NOT EXISTS "uuid-ossp" WITH SCHEMA extensions;

COMMENT ON EXTENSION "uuid-ossp" IS 'generate universally unique identifiers (UUIDs)';

CREATE FUNCTION auth.email() RETURNS text
    LANGUAGE sql STABLE
    AS $$
  select nullif(current_setting('request.jwt.claim.email', true), '')::text;
$$;

ALTER FUNCTION auth.email() OWNER TO postgres;

CREATE FUNCTION auth.role() RETURNS text
    LANGUAGE sql STABLE
    AS $$
  select nullif(current_setting('request.jwt.claim.role', true), '')::text;
$$;

ALTER FUNCTION auth.role() OWNER TO postgres;

CREATE FUNCTION auth.uid() RETURNS uuid
    LANGUAGE sql STABLE
    AS $$
  select nullif(current_setting('request.jwt.claim.sub', true), '')::uuid;
$$;

ALTER FUNCTION auth.uid() OWNER TO postgres;

CREATE FUNCTION extensions.notify_api_restart() RETURNS event_trigger
    LANGUAGE plpgsql
    AS $$
      BEGIN
        NOTIFY ddl_command_end;
      END;
      $$;

ALTER FUNCTION extensions.notify_api_restart() OWNER TO postgres;

COMMENT ON FUNCTION extensions.notify_api_restart() IS 'Sends a notification to the API to restart. If your database schema has changed, this is required so that Supabase can rebuild the relationships.';

CREATE FUNCTION pgbouncer.get_auth(p_usename text) RETURNS TABLE(username text, password text)
    LANGUAGE plpgsql SECURITY DEFINER
    AS $$
BEGIN
    RAISE WARNING 'PgBouncer auth request: %', p_usename;

    RETURN QUERY
    SELECT usename::TEXT, passwd::TEXT FROM pg_catalog.pg_shadow
    WHERE usename = p_usename;
END;
$$;

ALTER FUNCTION pgbouncer.get_auth(p_usename text) OWNER TO postgres;

CREATE FUNCTION public.update_page_views(url text) RETURNS void
    LANGUAGE plpgsql
    AS $$
begin
  if exists (select from "page_views" where "page" = "url") then
    update "page_views"
    set "views" = "views" + 1,
         "last_update" = now()
    where "page" = "url";
  else
    insert into "page_views"("page") values ("url");
  end if;
end;
$$;

ALTER FUNCTION public.update_page_views(url text) OWNER TO supabase_admin;

CREATE FUNCTION storage.extension(name text) RETURNS text
    LANGUAGE plpgsql
    AS $$
DECLARE
_parts text[];
_filename text;
BEGIN
	select string_to_array(name, '/') into _parts;
	select _parts[array_length(_parts,1)] into _filename;
	return split_part(_filename, '.', 2);
END
$$;

ALTER FUNCTION storage.extension(name text) OWNER TO supabase_storage_admin;

CREATE FUNCTION storage.filename(name text) RETURNS text
    LANGUAGE plpgsql
    AS $$
DECLARE
_parts text[];
BEGIN
	select string_to_array(name, '/') into _parts;
	return _parts[array_length(_parts,1)];
END
$$;

ALTER FUNCTION storage.filename(name text) OWNER TO supabase_storage_admin;

CREATE FUNCTION storage.foldername(name text) RETURNS text[]
    LANGUAGE plpgsql
    AS $$
DECLARE
_parts text[];
BEGIN
	select string_to_array(name, '/') into _parts;
	return _parts[1:array_length(_parts,1)-1];
END
$$;

ALTER FUNCTION storage.foldername(name text) OWNER TO supabase_storage_admin;

CREATE FUNCTION storage.get_size_by_bucket() RETURNS TABLE(size bigint, bucket_id text)
    LANGUAGE plpgsql
    AS $$
BEGIN
    return query
        select sum((metadata->>'size')::int) as size, obj.bucket_id
        from "storage".objects as obj
        group by obj.bucket_id;
END
$$;

ALTER FUNCTION storage.get_size_by_bucket() OWNER TO supabase_storage_admin;

CREATE FUNCTION storage.search(prefix text, bucketname text, limits integer DEFAULT 100, levels integer DEFAULT 1, offsets integer DEFAULT 0) RETURNS TABLE(name text, id uuid, updated_at timestamp with time zone, created_at timestamp with time zone, last_accessed_at timestamp with time zone, metadata jsonb)
    LANGUAGE plpgsql
    AS $$
BEGIN
	return query 
		with files_folders as (
			select path_tokens[levels] as folder
			from storage.objects
			where objects.name ilike prefix || '%'
			and bucket_id = bucketname
			GROUP by folder
			limit limits
			offset offsets
		) 
		select files_folders.folder as name, objects.id, objects.updated_at, objects.created_at, objects.last_accessed_at, objects.metadata from files_folders 
		left join storage.objects
		on prefix || files_folders.folder = objects.name
        where objects.id is null or objects.bucket_id=bucketname;
END
$$;

ALTER FUNCTION storage.search(prefix text, bucketname text, limits integer, levels integer, offsets integer) OWNER TO supabase_storage_admin;

SET default_tablespace = '';

SET default_table_access_method = heap;

CREATE TABLE auth.audit_log_entries (
    instance_id uuid,
    id uuid NOT NULL,
    payload json,
    created_at timestamp with time zone
);

ALTER TABLE auth.audit_log_entries OWNER TO supabase_auth_admin;

COMMENT ON TABLE auth.audit_log_entries IS 'Auth: Audit trail for user actions.';

CREATE TABLE auth.instances (
    id uuid NOT NULL,
    uuid uuid,
    raw_base_config text,
    created_at timestamp with time zone,
    updated_at timestamp with time zone
);

ALTER TABLE auth.instances OWNER TO supabase_auth_admin;

COMMENT ON TABLE auth.instances IS 'Auth: Manages users across multiple sites.';

CREATE TABLE auth.refresh_tokens (
    instance_id uuid,
    id bigint NOT NULL,
    token character varying(255),
    user_id character varying(255),
    revoked boolean,
    created_at timestamp with time zone,
    updated_at timestamp with time zone
);

ALTER TABLE auth.refresh_tokens OWNER TO supabase_auth_admin;

COMMENT ON TABLE auth.refresh_tokens IS 'Auth: Store of tokens used to refresh JWT tokens once they expire.';

CREATE SEQUENCE auth.refresh_tokens_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

ALTER TABLE auth.refresh_tokens_id_seq OWNER TO supabase_auth_admin;

ALTER SEQUENCE auth.refresh_tokens_id_seq OWNED BY auth.refresh_tokens.id;

CREATE TABLE auth.schema_migrations (
    version character varying(255) NOT NULL
);

ALTER TABLE auth.schema_migrations OWNER TO supabase_auth_admin;

COMMENT ON TABLE auth.schema_migrations IS 'Auth: Manages updates to the auth system.';

CREATE TABLE auth.users (
    instance_id uuid,
    id uuid NOT NULL,
    aud character varying(255),
    role character varying(255),
    email character varying(255),
    encrypted_password character varying(255),
    email_confirmed_at timestamp with time zone,
    invited_at timestamp with time zone,
    confirmation_token character varying(255),
    confirmation_sent_at timestamp with time zone,
    recovery_token character varying(255),
    recovery_sent_at timestamp with time zone,
    email_change_token_new character varying(255),
    email_change character varying(255),
    email_change_sent_at timestamp with time zone,
    last_sign_in_at timestamp with time zone,
    raw_app_meta_data jsonb,
    raw_user_meta_data jsonb,
    is_super_admin boolean,
    created_at timestamp with time zone,
    updated_at timestamp with time zone,
    phone character varying(15) DEFAULT NULL::character varying,
    phone_confirmed_at timestamp with time zone,
    phone_change character varying(15) DEFAULT ''::character varying,
    phone_change_token character varying(255) DEFAULT ''::character varying,
    phone_change_sent_at timestamp with time zone,
    confirmed_at timestamp with time zone GENERATED ALWAYS AS (LEAST(email_confirmed_at, phone_confirmed_at)) STORED,
    email_change_token_current character varying(255) DEFAULT ''::character varying,
    email_change_confirm_status smallint DEFAULT 0,
    CONSTRAINT users_email_change_confirm_status_check CHECK (((email_change_confirm_status >= 0) AND (email_change_confirm_status <= 2)))
);

ALTER TABLE auth.users OWNER TO supabase_auth_admin;

COMMENT ON TABLE auth.users IS 'Auth: Stores user login data within a secure schema.';

CREATE TABLE public.api_history (
    id bigint NOT NULL,
    date timestamp with time zone DEFAULT timezone('utc'::text, now()) NOT NULL,
    method text NOT NULL,
    route text NOT NULL,
    status_code text NOT NULL,
    status_message text NOT NULL
);

ALTER TABLE public.api_history OWNER TO supabase_admin;

ALTER TABLE public.api_history ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.api_history_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);

CREATE TABLE public.exceptions (
    id bigint NOT NULL,
    reported_at timestamp with time zone DEFAULT timezone('utc'::text, now()) NOT NULL,
    exception text NOT NULL
);

ALTER TABLE public.exceptions OWNER TO supabase_admin;

ALTER TABLE public.exceptions ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.exceptions_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);

CREATE TABLE public.page_views (
    id bigint NOT NULL,
    page text NOT NULL,
    views bigint DEFAULT 1 NOT NULL,
    last_update timestamp with time zone DEFAULT timezone('utc'::text, now()) NOT NULL
);

ALTER TABLE public.page_views OWNER TO supabase_admin;

ALTER TABLE public.page_views ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.page_views_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);

CREATE TABLE storage.buckets (
    id text NOT NULL,
    name text NOT NULL,
    owner uuid,
    created_at timestamp with time zone DEFAULT now(),
    updated_at timestamp with time zone DEFAULT now(),
    public boolean DEFAULT false
);

ALTER TABLE storage.buckets OWNER TO supabase_storage_admin;

CREATE TABLE storage.migrations (
    id integer NOT NULL,
    name character varying(100) NOT NULL,
    hash character varying(40) NOT NULL,
    executed_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP
);

ALTER TABLE storage.migrations OWNER TO supabase_storage_admin;

CREATE TABLE storage.objects (
    id uuid DEFAULT extensions.uuid_generate_v4() NOT NULL,
    bucket_id text,
    name text,
    owner uuid,
    created_at timestamp with time zone DEFAULT now(),
    updated_at timestamp with time zone DEFAULT now(),
    last_accessed_at timestamp with time zone DEFAULT now(),
    metadata jsonb,
    path_tokens text[] GENERATED ALWAYS AS (string_to_array(name, '/'::text)) STORED
);

ALTER TABLE storage.objects OWNER TO supabase_storage_admin;

ALTER TABLE ONLY auth.refresh_tokens ALTER COLUMN id SET DEFAULT nextval('auth.refresh_tokens_id_seq'::regclass);

ALTER TABLE ONLY auth.audit_log_entries
    ADD CONSTRAINT audit_log_entries_pkey PRIMARY KEY (id);

ALTER TABLE ONLY auth.instances
    ADD CONSTRAINT instances_pkey PRIMARY KEY (id);

ALTER TABLE ONLY auth.refresh_tokens
    ADD CONSTRAINT refresh_tokens_pkey PRIMARY KEY (id);

ALTER TABLE ONLY auth.schema_migrations
    ADD CONSTRAINT schema_migrations_pkey PRIMARY KEY (version);

ALTER TABLE ONLY auth.users
    ADD CONSTRAINT users_email_key UNIQUE (email);

ALTER TABLE ONLY auth.users
    ADD CONSTRAINT users_phone_key UNIQUE (phone);

ALTER TABLE ONLY auth.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (id);

ALTER TABLE ONLY public.api_history
    ADD CONSTRAINT api_history_pkey PRIMARY KEY (id);

ALTER TABLE ONLY public.exceptions
    ADD CONSTRAINT exceptions_pkey PRIMARY KEY (id);

ALTER TABLE ONLY public.page_views
    ADD CONSTRAINT page_views_page_key UNIQUE (page);

ALTER TABLE ONLY public.page_views
    ADD CONSTRAINT page_views_pkey PRIMARY KEY (id);

ALTER TABLE ONLY storage.buckets
    ADD CONSTRAINT buckets_pkey PRIMARY KEY (id);

ALTER TABLE ONLY storage.migrations
    ADD CONSTRAINT migrations_name_key UNIQUE (name);

ALTER TABLE ONLY storage.migrations
    ADD CONSTRAINT migrations_pkey PRIMARY KEY (id);

ALTER TABLE ONLY storage.objects
    ADD CONSTRAINT objects_pkey PRIMARY KEY (id);

CREATE INDEX audit_logs_instance_id_idx ON auth.audit_log_entries USING btree (instance_id);

CREATE INDEX refresh_tokens_instance_id_idx ON auth.refresh_tokens USING btree (instance_id);

CREATE INDEX refresh_tokens_instance_id_user_id_idx ON auth.refresh_tokens USING btree (instance_id, user_id);

CREATE INDEX refresh_tokens_token_idx ON auth.refresh_tokens USING btree (token);

CREATE INDEX users_instance_id_email_idx ON auth.users USING btree (instance_id, email);

CREATE INDEX users_instance_id_idx ON auth.users USING btree (instance_id);

CREATE UNIQUE INDEX bname ON storage.buckets USING btree (name);

CREATE UNIQUE INDEX bucketid_objname ON storage.objects USING btree (bucket_id, name);

CREATE INDEX name_prefix_search ON storage.objects USING btree (name text_pattern_ops);

ALTER TABLE ONLY storage.buckets
    ADD CONSTRAINT buckets_owner_fkey FOREIGN KEY (owner) REFERENCES auth.users(id);

ALTER TABLE ONLY storage.objects
    ADD CONSTRAINT "objects_bucketId_fkey" FOREIGN KEY (bucket_id) REFERENCES storage.buckets(id);

ALTER TABLE ONLY storage.objects
    ADD CONSTRAINT objects_owner_fkey FOREIGN KEY (owner) REFERENCES auth.users(id);

ALTER TABLE storage.buckets ENABLE ROW LEVEL SECURITY;

ALTER TABLE storage.migrations ENABLE ROW LEVEL SECURITY;

ALTER TABLE storage.objects ENABLE ROW LEVEL SECURITY;

CREATE PUBLICATION supabase_realtime WITH (publish = 'insert, update, delete, truncate');

ALTER PUBLICATION supabase_realtime OWNER TO postgres;

GRANT ALL ON SCHEMA auth TO supabase_auth_admin;
GRANT ALL ON SCHEMA auth TO dashboard_user;
GRANT ALL ON SCHEMA auth TO postgres;

GRANT USAGE ON SCHEMA extensions TO anon;
GRANT USAGE ON SCHEMA extensions TO authenticated;
GRANT USAGE ON SCHEMA extensions TO service_role;
GRANT ALL ON SCHEMA extensions TO dashboard_user;

GRANT USAGE ON SCHEMA public TO anon;
GRANT USAGE ON SCHEMA public TO authenticated;
GRANT USAGE ON SCHEMA public TO service_role;

GRANT ALL ON SCHEMA storage TO postgres;
GRANT USAGE ON SCHEMA storage TO anon;
GRANT USAGE ON SCHEMA storage TO authenticated;
GRANT USAGE ON SCHEMA storage TO service_role;
GRANT ALL ON SCHEMA storage TO supabase_storage_admin;
GRANT ALL ON SCHEMA storage TO dashboard_user;

REVOKE ALL ON FUNCTION pgbouncer.get_auth(p_usename text) FROM PUBLIC;
GRANT ALL ON FUNCTION pgbouncer.get_auth(p_usename text) TO pgbouncer;

GRANT ALL ON FUNCTION public.update_page_views(url text) TO postgres;
GRANT ALL ON FUNCTION public.update_page_views(url text) TO anon;
GRANT ALL ON FUNCTION public.update_page_views(url text) TO authenticated;
GRANT ALL ON FUNCTION public.update_page_views(url text) TO service_role;

GRANT ALL ON FUNCTION storage.extension(name text) TO anon;
GRANT ALL ON FUNCTION storage.extension(name text) TO authenticated;
GRANT ALL ON FUNCTION storage.extension(name text) TO service_role;

GRANT ALL ON FUNCTION storage.filename(name text) TO anon;
GRANT ALL ON FUNCTION storage.filename(name text) TO authenticated;
GRANT ALL ON FUNCTION storage.filename(name text) TO service_role;

GRANT ALL ON FUNCTION storage.foldername(name text) TO anon;
GRANT ALL ON FUNCTION storage.foldername(name text) TO authenticated;
GRANT ALL ON FUNCTION storage.foldername(name text) TO service_role;

GRANT ALL ON FUNCTION storage.search(prefix text, bucketname text, limits integer, levels integer, offsets integer) TO anon;
GRANT ALL ON FUNCTION storage.search(prefix text, bucketname text, limits integer, levels integer, offsets integer) TO authenticated;
GRANT ALL ON FUNCTION storage.search(prefix text, bucketname text, limits integer, levels integer, offsets integer) TO service_role;

GRANT ALL ON TABLE auth.audit_log_entries TO dashboard_user;
GRANT ALL ON TABLE auth.audit_log_entries TO postgres;

GRANT ALL ON TABLE auth.instances TO dashboard_user;
GRANT ALL ON TABLE auth.instances TO postgres;

GRANT ALL ON TABLE auth.refresh_tokens TO dashboard_user;
GRANT ALL ON TABLE auth.refresh_tokens TO postgres;

GRANT ALL ON TABLE auth.schema_migrations TO dashboard_user;
GRANT ALL ON TABLE auth.schema_migrations TO postgres;

GRANT ALL ON TABLE auth.users TO dashboard_user;
GRANT ALL ON TABLE auth.users TO postgres;

GRANT ALL ON TABLE extensions.pg_stat_statements TO dashboard_user;

GRANT ALL ON TABLE public.api_history TO postgres;
GRANT ALL ON TABLE public.api_history TO anon;
GRANT ALL ON TABLE public.api_history TO authenticated;
GRANT ALL ON TABLE public.api_history TO service_role;

GRANT ALL ON SEQUENCE public.api_history_id_seq TO postgres;
GRANT ALL ON SEQUENCE public.api_history_id_seq TO anon;
GRANT ALL ON SEQUENCE public.api_history_id_seq TO authenticated;
GRANT ALL ON SEQUENCE public.api_history_id_seq TO service_role;

GRANT ALL ON TABLE public.exceptions TO postgres;
GRANT ALL ON TABLE public.exceptions TO anon;
GRANT ALL ON TABLE public.exceptions TO authenticated;
GRANT ALL ON TABLE public.exceptions TO service_role;

GRANT ALL ON SEQUENCE public.exceptions_id_seq TO postgres;
GRANT ALL ON SEQUENCE public.exceptions_id_seq TO anon;
GRANT ALL ON SEQUENCE public.exceptions_id_seq TO authenticated;
GRANT ALL ON SEQUENCE public.exceptions_id_seq TO service_role;

GRANT ALL ON TABLE public.page_views TO postgres;
GRANT ALL ON TABLE public.page_views TO anon;
GRANT ALL ON TABLE public.page_views TO authenticated;
GRANT ALL ON TABLE public.page_views TO service_role;

GRANT ALL ON SEQUENCE public.page_views_id_seq TO postgres;
GRANT ALL ON SEQUENCE public.page_views_id_seq TO anon;
GRANT ALL ON SEQUENCE public.page_views_id_seq TO authenticated;
GRANT ALL ON SEQUENCE public.page_views_id_seq TO service_role;

GRANT ALL ON TABLE storage.buckets TO anon;
GRANT ALL ON TABLE storage.buckets TO authenticated;
GRANT ALL ON TABLE storage.buckets TO service_role;
GRANT ALL ON TABLE storage.buckets TO postgres;

GRANT ALL ON TABLE storage.migrations TO anon;
GRANT ALL ON TABLE storage.migrations TO authenticated;
GRANT ALL ON TABLE storage.migrations TO service_role;
GRANT ALL ON TABLE storage.migrations TO postgres;

GRANT ALL ON TABLE storage.objects TO anon;
GRANT ALL ON TABLE storage.objects TO authenticated;
GRANT ALL ON TABLE storage.objects TO service_role;
GRANT ALL ON TABLE storage.objects TO postgres;

ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA public REVOKE ALL ON SEQUENCES  FROM postgres;
ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA public GRANT ALL ON SEQUENCES  TO anon;
ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA public GRANT ALL ON SEQUENCES  TO authenticated;
ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA public GRANT ALL ON SEQUENCES  TO service_role;

ALTER DEFAULT PRIVILEGES FOR ROLE supabase_admin IN SCHEMA public REVOKE ALL ON SEQUENCES  FROM supabase_admin;
ALTER DEFAULT PRIVILEGES FOR ROLE supabase_admin IN SCHEMA public GRANT ALL ON SEQUENCES  TO postgres;
ALTER DEFAULT PRIVILEGES FOR ROLE supabase_admin IN SCHEMA public GRANT ALL ON SEQUENCES  TO anon;
ALTER DEFAULT PRIVILEGES FOR ROLE supabase_admin IN SCHEMA public GRANT ALL ON SEQUENCES  TO authenticated;
ALTER DEFAULT PRIVILEGES FOR ROLE supabase_admin IN SCHEMA public GRANT ALL ON SEQUENCES  TO service_role;

ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA public REVOKE ALL ON FUNCTIONS  FROM PUBLIC;
ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA public REVOKE ALL ON FUNCTIONS  FROM postgres;
ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA public GRANT ALL ON FUNCTIONS  TO anon;
ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA public GRANT ALL ON FUNCTIONS  TO authenticated;
ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA public GRANT ALL ON FUNCTIONS  TO service_role;

ALTER DEFAULT PRIVILEGES FOR ROLE supabase_admin IN SCHEMA public REVOKE ALL ON FUNCTIONS  FROM PUBLIC;
ALTER DEFAULT PRIVILEGES FOR ROLE supabase_admin IN SCHEMA public REVOKE ALL ON FUNCTIONS  FROM supabase_admin;
ALTER DEFAULT PRIVILEGES FOR ROLE supabase_admin IN SCHEMA public GRANT ALL ON FUNCTIONS  TO postgres;
ALTER DEFAULT PRIVILEGES FOR ROLE supabase_admin IN SCHEMA public GRANT ALL ON FUNCTIONS  TO anon;
ALTER DEFAULT PRIVILEGES FOR ROLE supabase_admin IN SCHEMA public GRANT ALL ON FUNCTIONS  TO authenticated;
ALTER DEFAULT PRIVILEGES FOR ROLE supabase_admin IN SCHEMA public GRANT ALL ON FUNCTIONS  TO service_role;

ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA public REVOKE ALL ON TABLES  FROM postgres;
ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA public GRANT ALL ON TABLES  TO anon;
ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA public GRANT ALL ON TABLES  TO authenticated;
ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA public GRANT ALL ON TABLES  TO service_role;

ALTER DEFAULT PRIVILEGES FOR ROLE supabase_admin IN SCHEMA public REVOKE ALL ON TABLES  FROM supabase_admin;
ALTER DEFAULT PRIVILEGES FOR ROLE supabase_admin IN SCHEMA public GRANT ALL ON TABLES  TO postgres;
ALTER DEFAULT PRIVILEGES FOR ROLE supabase_admin IN SCHEMA public GRANT ALL ON TABLES  TO anon;
ALTER DEFAULT PRIVILEGES FOR ROLE supabase_admin IN SCHEMA public GRANT ALL ON TABLES  TO authenticated;
ALTER DEFAULT PRIVILEGES FOR ROLE supabase_admin IN SCHEMA public GRANT ALL ON TABLES  TO service_role;

ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA storage REVOKE ALL ON SEQUENCES  FROM postgres;
ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA storage GRANT ALL ON SEQUENCES  TO anon;
ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA storage GRANT ALL ON SEQUENCES  TO authenticated;
ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA storage GRANT ALL ON SEQUENCES  TO service_role;

ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA storage REVOKE ALL ON FUNCTIONS  FROM PUBLIC;
ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA storage REVOKE ALL ON FUNCTIONS  FROM postgres;
ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA storage GRANT ALL ON FUNCTIONS  TO anon;
ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA storage GRANT ALL ON FUNCTIONS  TO authenticated;
ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA storage GRANT ALL ON FUNCTIONS  TO service_role;

ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA storage REVOKE ALL ON TABLES  FROM postgres;
ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA storage GRANT ALL ON TABLES  TO anon;
ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA storage GRANT ALL ON TABLES  TO authenticated;
ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA storage GRANT ALL ON TABLES  TO service_role;

CREATE EVENT TRIGGER api_restart ON ddl_command_end
   EXECUTE FUNCTION extensions.notify_api_restart();

ALTER EVENT TRIGGER api_restart OWNER TO supabase_admin;

CREATE EXTENSION file_fdw;

CREATE SERVER logserver FOREIGN DATA WRAPPER file_fdw;

CREATE FOREIGN TABLE pg_log (
  log_time timestamp(3) with time zone,
  user_name text,
  database_name text,
  process_id integer,
  connection_from text,
  session_id text,
  session_line_num bigint,
  command_tag text,
  session_start_time timestamp with time zone,
  virtual_transaction_id text,
  transaction_id bigint,
  error_severity text,
  sql_state_code text,
  message text,
  detail text,
  hint text,
  internal_query text,
  internal_query_pos integer,
  context text,
  query text,
  query_pos integer,
  location text,
  application_name text
) SERVER logserver
OPTIONS ( filename 'pg_log/postgresql.csv', format 'csv' );