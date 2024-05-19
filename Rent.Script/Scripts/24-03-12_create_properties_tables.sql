CREATE TABLE public."Properties"
(
    "Id" bigint NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 9223372036854775807 CACHE 1 ),
    "TypeProperties" integer NOT NULL,
    "Name" character varying NOT NULL,
    "Values" character varying
);

ALTER TABLE IF EXISTS public."Properties"
    OWNER to postgres;

ALTER TABLE IF EXISTS public."Properties"
    ADD PRIMARY KEY ("Id");

ALTER TABLE IF EXISTS public."Properties"
    ADD COLUMN "Uid" uuid NOT NULL;

ALTER TABLE IF EXISTS public."Properties"
    ADD COLUMN "IsRequired" boolean NOT NULL;


CREATE TABLE public."RentItemPropertiesConnection"
(
    "Id" bigint NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 9223372036854775807 CACHE 1 ),
    "RentItemId" bigint NOT NULL,
    "PropertiesId" bigint NOT NULL,
    "IntValue" integer,
    "StringValue" character varying,
    "BooleanValue" boolean,
    "DecimalValue" numeric,
    "DoubleValue" double precision,
    CONSTRAINT "PK_RentItemPropertiesConnection" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_RentItemPropertiesConnection_RentItem" FOREIGN KEY ("RentItemId")
        REFERENCES public."RentItem" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID,
    CONSTRAINT "FK_RentItemPropertiesConnection_Properties" FOREIGN KEY ("PropertiesId")
        REFERENCES public."Properties" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID
);

ALTER TABLE IF EXISTS public."RentItemPropertiesConnection"
    OWNER to postgres;

ALTER TABLE IF EXISTS public."RentItemPropertiesConnection"
    ADD COLUMN "Uid" uuid NOT NULL;



CREATE TABLE public."ChapterPropertiesConnection"
(
    "Id" bigint NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 9223372036854775807 ),
    "ChapterId" bigint NOT NULL,
    "PropertiesId" bigint NOT NULL,
    CONSTRAINT "PK_ChapterPropertiesConnection" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ChapterPropertiesConnection_Chapter" FOREIGN KEY ("ChapterId")
        REFERENCES public."Chapter" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID,
    CONSTRAINT "FK_ChapterPropertiesConnection_Properties" FOREIGN KEY ("PropertiesId")
        REFERENCES public."Properties" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID
);

ALTER TABLE IF EXISTS public."ChapterPropertiesConnection"
    OWNER to postgres;

ALTER TABLE IF EXISTS public."ChapterPropertiesConnection"
    ADD COLUMN "Uid" uuid NOT NULL;