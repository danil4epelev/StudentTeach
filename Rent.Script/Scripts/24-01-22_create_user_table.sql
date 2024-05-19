CREATE TABLE public."User"
(
    "Id" bigint NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 9223372036854775807 CACHE 1 ),
    "Login" character varying(1024) NOT NULL,
    "Name" character varying NOT NULL,
    "LastName" character varying NOT NULL,
    "MiddleName" character varying,
    "Uid" uuid NOT NULL,
    "Email" character varying(256) NOT NULL,
    "PhoneNumber" character varying,
    "RoleType" integer NOT NULL,
    PRIMARY KEY ("Id")
);

ALTER TABLE IF EXISTS public."User"
    OWNER to postgres;

ALTER TABLE IF EXISTS public."User"
    ADD COLUMN "PasswordHash" character varying NOT NULL;

ALTER TABLE IF EXISTS public."User"
    ALTER COLUMN "LastName" DROP NOT NULL;

ALTER TABLE IF EXISTS public."User"
    ALTER COLUMN "Name" DROP NOT NULL;