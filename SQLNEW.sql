--------------------------------------------------------
--  File created - Sunday-May-11-2025   
--------------------------------------------------------
--------------------------------------------------------
--  DDL for Type BOOK_BORROWER_OBJ
--------------------------------------------------------

  CREATE OR REPLACE TYPE "BOOK_BORROWER_OBJ" as OBJECT (
        borrower_fn VARCHAR2(25),
        borrower_ln VARCHAR2(25),
        borrower_id VARCHAR2(25)
    );

/
--------------------------------------------------------
--  DDL for Type USER_OBJ
--------------------------------------------------------

  CREATE OR REPLACE TYPE "USER_OBJ" as OBJECT (
    firstname VARCHAR2(25),
    lastname VARCHAR2(25),
    email VARCHAR2(25),
    Datecreated DATE
)

/
--------------------------------------------------------
--  DDL for Sequence BOOK_INCREMENT_ONE
--------------------------------------------------------

   CREATE SEQUENCE  "BOOK_INCREMENT_ONE"  MINVALUE 1 MAXVALUE 9999999999999999999999999999 INCREMENT BY 1 START WITH 101 CACHE 20 NOORDER  NOCYCLE ;
--------------------------------------------------------
--  DDL for Sequence BORROW_BOOK_INCREMENT_ONE
--------------------------------------------------------

   CREATE SEQUENCE  "BORROW_BOOK_INCREMENT_ONE"  MINVALUE 1 MAXVALUE 999999999 INCREMENT BY 1 START WITH 101 CACHE 20 NOORDER  NOCYCLE ;
--------------------------------------------------------
--  DDL for Table BOOKS
--------------------------------------------------------

  CREATE TABLE "BOOKS" 
   (	"TITLE" VARCHAR2(255 BYTE), 
	"AUTHOR" VARCHAR2(255 BYTE), 
	"PUBLISHER" VARCHAR2(255 BYTE), 
	"PUBLICATION_DATE" DATE, 
	"GENRE" VARCHAR2(255 BYTE), 
	"BOOK_LANGUAGE" VARCHAR2(50 BYTE), 
	"PAGE_COUNT" NUMBER(*,0), 
	"QUANTITY" NUMBER(*,0), 
	"LAST_UPDATED" TIMESTAMP (6), 
	"BOOK_ID" NUMBER, 
	"BRANCH" VARCHAR2(20 BYTE)
   ) SEGMENT CREATION IMMEDIATE 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1 BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
   ;
--------------------------------------------------------
--  DDL for Table BORROWED_BOOKS
--------------------------------------------------------

  CREATE TABLE "BORROWED_BOOKS" 
   (	"BORROW_ID" NUMBER(*,0), 
	"BORROWER_ID" VARCHAR2(10 BYTE), 
	"BORROWER_LN" VARCHAR2(25 BYTE), 
	"BORROWER_FN" VARCHAR2(25 BYTE), 
	"BORROW_DUE" DATE, 
	"BORROW_DATE" DATE, 
	"STATUS" VARCHAR2(20 BYTE), 
	"BOOK_ID" NUMBER, 
	"BRANCH" VARCHAR2(20 BYTE), 
	"EMAIL" VARCHAR2(50 BYTE)
   ) SEGMENT CREATION IMMEDIATE 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1 BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
   ;
--------------------------------------------------------
--  DDL for Table LOGS
--------------------------------------------------------

  CREATE TABLE "LOGS" 
   (	"EMAIL" VARCHAR2(50 BYTE), 
	"STUDENT_ID" VARCHAR2(20 BYTE), 
	"DATE_LOGGED" DATE, 
	"TIME_LOGGED" TIMESTAMP (6), 
	"TYPE" VARCHAR2(20 BYTE), 
	"BRANCH" VARCHAR2(20 BYTE)
   ) SEGMENT CREATION IMMEDIATE 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1 BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
   ;
--------------------------------------------------------
--  DDL for Table USERS
--------------------------------------------------------

  CREATE TABLE "USERS" 
   (	"STUDENT_ID" VARCHAR2(10 BYTE), 
	"FIRST_NAME" VARCHAR2(25 BYTE), 
	"LAST_NAME" VARCHAR2(25 BYTE), 
	"EMAIL" VARCHAR2(50 BYTE), 
	"DATE_CREATED" TIMESTAMP (6), 
	"TYPE" VARCHAR2(20 BYTE), 
	"BRANCH" VARCHAR2(20 BYTE), 
	"PASSWORD" VARCHAR2(20 BYTE)
   ) SEGMENT CREATION IMMEDIATE 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1 BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
   ;
REM INSERTING into XEROJ.BOOKS
SET DEFINE OFF;
REM INSERTING into XEROJ.BORROWED_BOOKS
SET DEFINE OFF;
REM INSERTING into XEROJ.LOGS
SET DEFINE OFF;
REM INSERTING into XEROJ.USERS
SET DEFINE OFF;
--------------------------------------------------------
--  DDL for Index SYS_C007091
--------------------------------------------------------

  CREATE UNIQUE INDEX "SYS_C007091" ON "BORROWED_BOOKS" ("BORROW_ID") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1 BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
   ;
--------------------------------------------------------
--  DDL for Index BOOK_TITLE_INDEX
--------------------------------------------------------

  CREATE UNIQUE INDEX "BOOK_TITLE_INDEX" ON "BOOKS" ("TITLE") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1 BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
   ;
--------------------------------------------------------
--  DDL for Index BOOKS_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "BOOKS_PK" ON "BOOKS" ("BOOK_ID") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1 BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
   ;
--------------------------------------------------------
--  DDL for Trigger BOOK_ID_AI
--------------------------------------------------------

  CREATE OR REPLACE TRIGGER "BOOK_ID_AI" 
BEFORE INSERT ON books
FOR EACH ROW
BEGIN
    :NEW.book_ID := book_increment_one.NEXTVAL;
END;
/
ALTER TRIGGER "BOOK_ID_AI" ENABLE;
--------------------------------------------------------
--  DDL for Trigger BOOK_UPDATED
--------------------------------------------------------

  CREATE OR REPLACE TRIGGER "BOOK_UPDATED" 
BEFORE INSERT OR UPDATE ON books
FOR EACH ROW
BEGIN
    :new.last_updated := CURRENT_TIMESTAMP;
END;
/
ALTER TRIGGER "BOOK_UPDATED" ENABLE;
--------------------------------------------------------
--  DDL for Trigger BORROWED_BOOKS_BIR
--------------------------------------------------------

  CREATE OR REPLACE TRIGGER "BORROWED_BOOKS_BIR" 
BEFORE INSERT ON borrowed_books
FOR EACH ROW
BEGIN
    SELECT borrow_book_increment_one.NEXTVAL INTO :new.Borrow_id FROM dual;
END;
/
ALTER TRIGGER "BORROWED_BOOKS_BIR" ENABLE;
--------------------------------------------------------
--  DDL for Trigger USER_CREATION_DATE
--------------------------------------------------------

  CREATE OR REPLACE TRIGGER "USER_CREATION_DATE" 
BEFORE INSERT OR UPDATE on users
FOR EACH ROW
BEGIN
    :new.date_created := CURRENT_TIMESTAMP;
END;
/
ALTER TRIGGER "USER_CREATION_DATE" ENABLE;
--------------------------------------------------------
--  DDL for Procedure ADDNUM
--------------------------------------------------------
set define off;

  CREATE OR REPLACE PROCEDURE "ADDNUM" (
    num1 IN NUMBER,
    num2 IN NUMBER,
    result OUT NUMBER
)
IS
BEGIN
    result := num1 + num2;
END;

/
--------------------------------------------------------
--  DDL for Procedure DOUBLE_VALUE
--------------------------------------------------------
set define off;

  CREATE OR REPLACE PROCEDURE "DOUBLE_VALUE" (num IN OUT NUMBER) IS
BEGIN
    num := num * 2;
END double_value;

/
--------------------------------------------------------
--  DDL for Procedure MULTIPLY_NUMBERS
--------------------------------------------------------
set define off;

  CREATE OR REPLACE PROCEDURE "MULTIPLY_NUMBERS" (
    a IN NUMBER,
    b IN NUMBER,
    result OUT NUMBER
) IS
BEGIN
    result := a * b;
END multiply_numbers;

/
--------------------------------------------------------
--  DDL for Procedure PRINTNAME
--------------------------------------------------------
set define off;

  CREATE OR REPLACE PROCEDURE "PRINTNAME" (name IN VARCHAR2)
IS
BEGIN
    DBMS_OUTPUT.PUT_LINE('Name Entered Is: ' || name);
END;

/
--------------------------------------------------------
--  DDL for Procedure PRINT_SQUARE
--------------------------------------------------------
set define off;

  CREATE OR REPLACE PROCEDURE "PRINT_SQUARE" (num IN NUMBER) IS
BEGIN
    DBMS_OUTPUT.PUT_LINE('Square is: ' || (num * num));
END print_square;

/
--------------------------------------------------------
--  DDL for Procedure SAY_HELLO
--------------------------------------------------------
set define off;

  CREATE OR REPLACE PROCEDURE "SAY_HELLO" IS
BEGIN
    DBMS_OUTPUT.PUT_LINE('Hello from PL/SQL!');
END say_hello;

/
--------------------------------------------------------
--  DDL for Function GETBOOKBORROWER
--------------------------------------------------------

  CREATE OR REPLACE FUNCTION "GETBOOKBORROWER" (bid NUMBER)
        RETURN book_borrower_obj
    IS 
        v_borrowerInfo book_borrower_obj;
    BEGIN
        SELECT book_borrower_obj(borrower_fn, borrower_ln, borrower_id)
        INTO v_borrowerInfo
        FROM borrowed_books
        WHERE book_id = bid;

        RETURN v_borrowerInfo;
    END;

/
--------------------------------------------------------
--  DDL for Function GETBOOKTITLE
--------------------------------------------------------

  CREATE OR REPLACE FUNCTION "GETBOOKTITLE" (bid NUMBER)
        RETURN VARCHAR2 
    IS 
        v_bookTitle VARCHAR2(100);
    BEGIN
        SELECT title
        INTO v_bookTitle
        FROM books
        WHERE book_id = bid;

        RETURN v_bookTitle;
    END;

/
--------------------------------------------------------
--  DDL for Function GETUSER
--------------------------------------------------------

  CREATE OR REPLACE FUNCTION "GETUSER" (uid VARCHAR2)
    RETURN user_obj IS user_details user_obj;
BEGIN
    SELECT user_obj(first_name, last_name, email, date_created)
    into user_details
    FROM users
    WHERE student_id = uid;

    RETURN user_details;
END;

/
--------------------------------------------------------
--  Constraints for Table BOOKS
--------------------------------------------------------

  ALTER TABLE "BOOKS" MODIFY ("BRANCH" NOT NULL ENABLE);
  ALTER TABLE "BOOKS" ADD CONSTRAINT "BOOKS_PK" PRIMARY KEY ("BOOK_ID")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1 BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
    ENABLE;
  ALTER TABLE "BOOKS" MODIFY ("BOOK_ID" NOT NULL ENABLE);
  ALTER TABLE "BOOKS" MODIFY ("BOOK_LANGUAGE" NOT NULL ENABLE);
  ALTER TABLE "BOOKS" MODIFY ("GENRE" NOT NULL ENABLE);
  ALTER TABLE "BOOKS" MODIFY ("PUBLICATION_DATE" NOT NULL ENABLE);
  ALTER TABLE "BOOKS" MODIFY ("PUBLISHER" NOT NULL ENABLE);
  ALTER TABLE "BOOKS" MODIFY ("AUTHOR" NOT NULL ENABLE);
  ALTER TABLE "BOOKS" MODIFY ("TITLE" NOT NULL ENABLE);
  ALTER TABLE "BOOKS" MODIFY ("QUANTITY" NOT NULL ENABLE);
--------------------------------------------------------
--  Constraints for Table LOGS
--------------------------------------------------------

  ALTER TABLE "LOGS" MODIFY ("BRANCH" NOT NULL ENABLE);
  ALTER TABLE "LOGS" MODIFY ("TYPE" NOT NULL ENABLE);
  ALTER TABLE "LOGS" MODIFY ("TIME_LOGGED" NOT NULL ENABLE);
  ALTER TABLE "LOGS" MODIFY ("DATE_LOGGED" NOT NULL ENABLE);
  ALTER TABLE "LOGS" MODIFY ("EMAIL" NOT NULL ENABLE);
--------------------------------------------------------
--  Constraints for Table USERS
--------------------------------------------------------

  ALTER TABLE "USERS" MODIFY ("BRANCH" NOT NULL ENABLE);
  ALTER TABLE "USERS" MODIFY ("TYPE" NOT NULL ENABLE);
  ALTER TABLE "USERS" MODIFY ("PASSWORD" NOT NULL ENABLE);
  ALTER TABLE "USERS" MODIFY ("EMAIL" NOT NULL ENABLE);
  ALTER TABLE "USERS" ADD PRIMARY KEY ("STUDENT_ID")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1 BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
    ENABLE;
  ALTER TABLE "USERS" MODIFY ("LAST_NAME" NOT NULL ENABLE);
  ALTER TABLE "USERS" MODIFY ("FIRST_NAME" NOT NULL ENABLE);
--------------------------------------------------------
--  Constraints for Table BORROWED_BOOKS
--------------------------------------------------------

  ALTER TABLE "BORROWED_BOOKS" MODIFY ("EMAIL" NOT NULL ENABLE);
  ALTER TABLE "BORROWED_BOOKS" MODIFY ("BRANCH" NOT NULL ENABLE);
  ALTER TABLE "BORROWED_BOOKS" MODIFY ("BORROWER_ID" NOT NULL ENABLE);
  ALTER TABLE "BORROWED_BOOKS" MODIFY ("STATUS" NOT NULL ENABLE);
  ALTER TABLE "BORROWED_BOOKS" ADD CONSTRAINT "SYS_C007091" PRIMARY KEY ("BORROW_ID")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1 BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
    ENABLE;
  ALTER TABLE "BORROWED_BOOKS" MODIFY ("BORROW_DUE" NOT NULL ENABLE);
  ALTER TABLE "BORROWED_BOOKS" MODIFY ("BOOK_ID" NOT NULL ENABLE);
--------------------------------------------------------
--  Ref Constraints for Table BORROWED_BOOKS
--------------------------------------------------------

  ALTER TABLE "BORROWED_BOOKS" ADD CONSTRAINT "FK_BOOK_ID" FOREIGN KEY ("BOOK_ID")
	  REFERENCES "BOOKS" ("BOOK_ID") ON DELETE CASCADE ENABLE;
  ALTER TABLE "BORROWED_BOOKS" ADD CONSTRAINT "FK_BORROWER_ID" FOREIGN KEY ("BORROWER_ID")
	  REFERENCES "USERS" ("STUDENT_ID") ON DELETE CASCADE ENABLE;
