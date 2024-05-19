CREATE TABLE public."Chapter"
(
    "Id" bigint NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 9223372036854775807 CACHE 1 ),
    "Uid" uuid NOT NULL,
    "Name" character varying NOT NULL,
    "ParentChapterId" bigint,
    PRIMARY KEY ("Id")
);

ALTER TABLE IF EXISTS public."Chapter"
    OWNER to postgres;

ALTER TABLE IF EXISTS public."Chapter"
    ADD CONSTRAINT "FK_Chapter" FOREIGN KEY ("ParentChapterId")
    REFERENCES public."Chapter" ("Id") MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION
    NOT VALID;