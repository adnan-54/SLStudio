CREATE TABLE public.api_history (
    id bigint NOT NULL,
    date timestamp with time zone DEFAULT timezone('utc'::text, now()) NOT NULL,
    method text NOT NULL,
    route text NOT NULL,
    status_code text NOT NULL,
    status_message text NOT NULL
);

CREATE TABLE public.exceptions (
    id bigint NOT NULL,
    reported_at timestamp with time zone DEFAULT timezone('utc'::text, now()) NOT NULL,
    exception text NOT NULL
);

CREATE TABLE public.page_views (
    id bigint NOT NULL,
    page text NOT NULL,
    views bigint DEFAULT 1 NOT NULL,
    last_update timestamp with time zone DEFAULT timezone('utc'::text, now()) NOT NULL
);

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