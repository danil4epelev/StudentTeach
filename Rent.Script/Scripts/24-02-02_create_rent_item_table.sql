CREATE TABLE public."RentItem"
(
    "Id" bigint NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 9223372036854775807 CACHE 1 ),
    "Uid" uuid NOT NULL,
    "Name" character varying(256) NOT NULL,
    "Description" character varying(10000) NOT NULL,
    "ChapterId" bigint NOT NULL,
    "Price" numeric NOT NULL,
    "PriceType" integer NOT NULL,
    "AuthorId" bigint NOT NULL,
    "ModeratorId" bigint NULL,
    PRIMARY KEY ("Id"),
    CONSTRAINT "FK_RentItem_Chapter" FOREIGN KEY ("ChapterId")
        REFERENCES public."Chapter" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID,
    CONSTRAINT "FK_RentItem_Author" FOREIGN KEY ("AuthorId")
        REFERENCES public."User" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID,
    CONSTRAINT "FK_RentItem_Moderator" FOREIGN KEY ("ModeratorId")
        REFERENCES public."User" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID
);

ALTER TABLE IF EXISTS public."RentItem"
    OWNER to postgres;


ALTER TABLE IF EXISTS public."RentItem"
    ADD COLUMN "DtCreate" date NOT NULL;

ALTER TABLE IF EXISTS public."RentItem"
    ADD COLUMN "DtApprove" date;

ALTER TABLE IF EXISTS public."RentItem"
    ADD COLUMN "DtUpToSearch" date;

ALTER TABLE IF EXISTS public."RentItem"
    ADD COLUMN "Status" integer NOT NULL;

ALTER TABLE IF EXISTS public."RentItem"
    ADD COLUMN "DtSendToModeration" date;

ALTER TABLE IF EXISTS public."RentItem"
    ADD COLUMN "RejectedRemarks" character varying;

ALTER TABLE IF EXISTS public."RentItem"
    ADD COLUMN "DtReject" date;