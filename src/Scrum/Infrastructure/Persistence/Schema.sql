-- SQLite

-- $ sqlite3 scrum.sqlite < infrastructure/sql/20230427-initial.sql

-- TODO: Add cascading delete

create table stories(
    id text primary key,
    title text not null,
    description text null,
    created_at text not null,
    updated_at text not null
) strict;

create table tasks(
    id text primary key,
    story_id text not null, -- TODO: add index of foreign key
    title text not null,
    description text null,
    created_at text not null,
    updated_at text not null
) strict;

-- TODO: extend with more field from actual solution
create table domain_events(
    id text primary key,
    payload text not null
) strict;