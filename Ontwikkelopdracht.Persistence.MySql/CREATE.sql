-- Sven Dubbeld
-- Site: pathe.nl

-- DROP
SET FOREIGN_KEY_CHECKS = 0;
DROP TABLE IF EXISTS META;
DROP TABLE IF EXISTS CREW ;
DROP TABLE IF EXISTS FILM_CREW_MAP;
DROP TABLE IF EXISTS FILM;
DROP TABLE IF EXISTS VOORSTELLING;
DROP TABLE IF EXISTS EVENT;
DROP TABLE IF EXISTS CREW_TYPE;
DROP TABLE IF EXISTS FILM_KIJKWIJZER_MAP;
DROP TABLE IF EXISTS KIJKWIJZER_ITEM;
DROP TABLE IF EXISTS GENRE;
DROP TABLE IF EXISTS MEDIA_ITEM;
DROP TABLE IF EXISTS KAARTJE;
DROP TABLE IF EXISTS BLOG;
DROP TABLE IF EXISTS MEDIA_ITEM_SUGGESTION;
DROP TABLE IF EXISTS BESTELLING;
DROP TABLE IF EXISTS GEBRUIKER;
DROP TABLE IF EXISTS KORTINGSKAART_UITGAVE;
DROP TABLE IF EXISTS BIOSCOOP;
DROP TABLE IF EXISTS KORTINGSKAART;
DROP TABLE IF EXISTS ZAAL;
DROP TABLE IF EXISTS STOEL;
SET FOREIGN_KEY_CHECKS = 1;


-- CREATE

CREATE TABLE META (
  ID INT PRIMARY KEY AUTO_INCREMENT,
  `KEY` VARCHAR(256) NOT NULL UNIQUE,
  `VALUE` TEXT NOT NULL
);

CREATE TABLE CREW (
  CREW_ID INT PRIMARY KEY AUTO_INCREMENT,
  NAAM TEXT(100) NOT NULL,
  GEBOORTEDATUM DATE NOT NULL,
  GESLACHT INT(1) NOT NULL CHECK (GESLACHT IN (0, 1)),
  BIO LONGTEXT NOT NULL
);

CREATE TABLE CREW_TYPE (
  TYPE_ID INT PRIMARY KEY AUTO_INCREMENT,
  NAAM TEXT(100) NOT NULL
);

CREATE TABLE GENRE (
  GENRE_ID INT PRIMARY KEY AUTO_INCREMENT,
  NAAM TEXT(100) NOT NULL
);

CREATE TABLE FILM (
  FILM_ID INT PRIMARY KEY AUTO_INCREMENT,
  TITEL TEXT(100) NOT NULL,
  DUUR INT NOT NULL,
  `RELEASE` DATE NOT NULL,
  GESPROKEN_TAAL CHAR(2) NOT NULL,
  ONDERTITEL_TAAL CHAR(2),
  GENRE_ID INT NOT NULL,
  SEQUEL_TO INT,
  FOREIGN KEY (GENRE_ID) REFERENCES GENRE (GENRE_ID) ON DELETE CASCADE ON UPDATE CASCADE,
  FOREIGN KEY (SEQUEL_TO) REFERENCES FILM (FILM_ID) ON DELETE SET NULL ON UPDATE CASCADE
);

CREATE TABLE FILM_CREW_MAP (
  ID INT PRIMARY KEY AUTO_INCREMENT,
  FILM_ID INT,
  FOREIGN KEY (FILM_ID) REFERENCES FILM (FILM_ID) ON DELETE CASCADE ON UPDATE CASCADE,
  CREW_ID INT,
  FOREIGN KEY (CREW_ID) REFERENCES CREW (CREW_ID) ON DELETE CASCADE ON UPDATE CASCADE,
  TYPE_ID INT,
  FOREIGN KEY (TYPE_ID) REFERENCES CREW_TYPE (TYPE_ID) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT UNIQUE_FILM_CREW_MAP UNIQUE (FILM_ID, CREW_ID, TYPE_ID)
);

CREATE TABLE KIJKWIJZER_ITEM (
  KIJKWIJZER_ID INT PRIMARY KEY AUTO_INCREMENT,
  NAAM TEXT(100) NOT NULL,
  ICON BLOB NOT NULL
);

CREATE TABLE FILM_KIJKWIJZER_MAP (
  FILM_ID INT,
  FOREIGN KEY (FILM_ID) REFERENCES FILM (FILM_ID) ON DELETE CASCADE ON UPDATE CASCADE,
  KIJKWIJZER_ID INT,
  FOREIGN KEY (KIJKWIJZER_ID) REFERENCES KIJKWIJZER_ITEM (KIJKWIJZER_ID) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT PK_FILM_KIJKWIJZER_MAP PRIMARY KEY (FILM_ID, KIJKWIJZER_ID)
);

CREATE TABLE MEDIA_ITEM (
  ITEM_ID INT PRIMARY KEY AUTO_INCREMENT,
  FILM_ID INT REFERENCES FILM (FILM_ID),
  NAAM TEXT(100) NOT NULL,
  OMSCHRIJVING TEXT(500) NOT NULL,
  URL TEXT(2083) NOT NULL,
  TYPE TEXT(100) NOT NULL
);

CREATE TABLE MEDIA_ITEM_SUGGESTION (
  FIRST_ITEM_ID INT,
  FOREIGN KEY (FIRST_ITEM_ID) REFERENCES MEDIA_ITEM (ITEM_ID) ON DELETE CASCADE ON UPDATE CASCADE,
  SECOND_ITEM_ID INT,
  FOREIGN KEY (SECOND_ITEM_ID) REFERENCES MEDIA_ITEM (ITEM_ID) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT PK_MEDIA_ITEM_SUGGESTION PRIMARY KEY (FIRST_ITEM_ID, SECOND_ITEM_ID)
);

CREATE TABLE EVENT (
  EVENT_ID INT PRIMARY KEY AUTO_INCREMENT,
  NAAM TEXT(100) NOT NULL,
  BEGIN_DATUM DATE NOT NULL,
  EIND_DATUM DATE NOT NULL,
  CHECK (EIND_DATUM - BEGIN_DATUM >= 0)
);  

CREATE TABLE BIOSCOOP (
  BIOSCOOP_ID INT PRIMARY KEY AUTO_INCREMENT,
  NAAM TEXT(100) NOT NULL,
  STRAAT TEXT(100) NOT NULL,
  HUISNUMMER INT NOT NULL,
  POSTCODE TEXT(6) NOT NULL,
  PLAATS TEXT(100) NOT NULL
);

CREATE TABLE ZAAL (
  ZAAL_ID INT PRIMARY KEY AUTO_INCREMENT,
  NUMMER INT NOT NULL,
  BIOSCOOP_ID INT NOT NULL,
  FOREIGN KEY (BIOSCOOP_ID) REFERENCES BIOSCOOP (BIOSCOOP_ID) ON DELETE CASCADE ON UPDATE CASCADE
);  

CREATE TABLE STOEL (
  STOEL_ID INT PRIMARY KEY AUTO_INCREMENT,
  NUMMER INT NOT NULL,
  RIJ INT NOT NULL,
  ZAAL_ID INT NOT NULL,
  FOREIGN KEY (ZAAL_ID) REFERENCES ZAAL (ZAAL_ID) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE VOORSTELLING (
  VOORSTELLING_ID INT PRIMARY KEY AUTO_INCREMENT,
  FILM_ID INT NOT NULL,
  FOREIGN KEY (FILM_ID) REFERENCES FILM (FILM_ID) ON DELETE CASCADE ON UPDATE CASCADE,
  ZAAL_ID INT,
  FOREIGN KEY (ZAAL_ID) REFERENCES ZAAL (ZAAL_ID) ON DELETE CASCADE ON UPDATE CASCADE,
  EVENT_ID INT,
  FOREIGN KEY (EVENT_ID) REFERENCES EVENT (EVENT_ID) ON DELETE SET NULL ON UPDATE CASCADE,
  DATUM DATETIME NOT NULL
);  

CREATE TABLE GEBRUIKER (
  USER_ID INT PRIMARY KEY AUTO_INCREMENT,
  EMAIL VARCHAR(100) UNIQUE CHECK(REGEXP_LIKE (EMAIL,'^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$')),
  NAAM TEXT(100) NOT NULL,
  WACHTWOORD TEXT(2000) NOT NULL CHECK (UPPER(WACHTWOORD) != WACHTWOORD AND LOWER(WACHTWOORD) != WACHTWOORD AND LENGTH(WACHTWOORD) >= 8),
  ADMIN INT(1) NOT NULL DEFAULT 0
);

CREATE TABLE KORTINGSKAART (
  KAARTNR INT PRIMARY KEY AUTO_INCREMENT,
  USER_ID INT NOT NULL,
  FOREIGN KEY (USER_ID) REFERENCES GEBRUIKER (USER_ID) ON DELETE CASCADE ON UPDATE CASCADE,
  BEDRAG INT NOT NULL
);

CREATE TABLE BESTELLING (
  BESTELNR INT PRIMARY KEY AUTO_INCREMENT,
  USER_ID INT NOT NULL,
  FOREIGN KEY (USER_ID) REFERENCES GEBRUIKER (USER_ID) ON DELETE RESTRICT ON UPDATE CASCADE,
  DATUM DATETIME NOT NULL,
  PRIJS INT NOT NULL
);

CREATE TABLE KORTINGSKAART_UITGAVE (
  BESTELNR INT,
  FOREIGN KEY (BESTELNR) REFERENCES BESTELLING (BESTELNR) ON DELETE CASCADE ON UPDATE CASCADE,
  KAARTNR INT,
  FOREIGN KEY (KAARTNR) REFERENCES KORTINGSKAART (KAARTNR) ON DELETE CASCADE ON UPDATE CASCADE,
  BEDRAG INT NOT NULL,
  CONSTRAINT PK_KORTINGSKAART_UITGAVE PRIMARY KEY (BESTELNR, KAARTNR)
);

CREATE TABLE KAARTJE (
  KAARTJE_ID INT PRIMARY KEY AUTO_INCREMENT,
  VOORSTELLING_ID INT,
  FOREIGN KEY (VOORSTELLING_ID) REFERENCES VOORSTELLING (VOORSTELLING_ID) ON DELETE CASCADE ON UPDATE CASCADE,
  STOEL_ID INT,
  FOREIGN KEY (STOEL_ID) REFERENCES STOEL (STOEL_ID) ON DELETE CASCADE ON UPDATE CASCADE,
  BESTELLING_ID INT NOT NULL,
  FOREIGN KEY (BESTELLING_ID) REFERENCES BESTELLING (BESTELNR) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT PK_KAARTJE UNIQUE (VOORSTELLING_ID, STOEL_ID)
);

CREATE TABLE BLOG (
  BLOG_ID INT PRIMARY KEY AUTO_INCREMENT,
  TITEL TEXT(100) NOT NULL,
  DATUM DATETIME NOT NULL,
  BODY LONGTEXT NOT NULL,
  AUTEUR INT NOT NULL,
  FOREIGN KEY (AUTEUR) REFERENCES GEBRUIKER (USER_ID) ON DELETE RESTRICT ON UPDATE CASCADE
);

-- -- INSERT

-- META

INSERT INTO META (`KEY`, `VALUE`) VALUES ('DB_VERSION', '1');

-- CREW

insert into CREW (CREW_ID, NAAM, GEBOORTEDATUM, GESLACHT, BIO) values (1, 'Carlos Arnold', '1970-09-22', 0, 'Nam nulla. Integer pede justo, lacinia eget, tincidunt eget, tempus vel, pede. Morbi porttitor lorem id ligula. Suspendisse ornare consequat lectus. In est risus, auctor sed, tristique in, tempus sit amet, sem. Fusce consequat. Nulla nisl. Nunc nisl.');
insert into CREW (CREW_ID, NAAM, GEBOORTEDATUM, GESLACHT, BIO) values (2, 'Bruce Young', '1988-05-26', 0, 'Aliquam augue quam, sollicitudin vitae, consectetuer eget, rutrum at, lorem. Integer tincidunt ante vel ipsum. Praesent blandit lacinia erat. Vestibulum sed magna at nunc commodo placerat. Praesent blandit. Nam nulla.');
insert into CREW (CREW_ID, NAAM, GEBOORTEDATUM, GESLACHT, BIO) values (3, 'Adam Castillo', '1971-04-16', 0, 'Cras non velit nec nisi vulputate nonummy. Maecenas tincidunt lacus at velit. Vivamus vel nulla eget eros elementum pellentesque. Quisque porta volutpat erat. Quisque erat eros, viverra eget, congue eget, semper rutrum, nulla.');
insert into CREW (CREW_ID, NAAM, GEBOORTEDATUM, GESLACHT, BIO) values (4, 'Barbara Thomas', '1989-04-20', 1, 'Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Vivamus vestibulum sagittis sapien.');
insert into CREW (CREW_ID, NAAM, GEBOORTEDATUM, GESLACHT, BIO) values (5, 'Marilyn Hernandez', '1968-03-28', 1, 'Nullam orci pede, venenatis non, sodales sed, tincidunt eu, felis. Fusce posuere felis sed lacus. Morbi sem mauris, laoreet ut, rhoncus aliquet, pulvinar sed, nisl. Nunc rhoncus dui vel sem. Sed sagittis. Nam congue, risus semper porta volutpat, quam pede lobortis ligula, sit amet eleifend pede libero quis orci. Nullam molestie nibh in lectus. Pellentesque at nulla. Suspendisse potenti. Cras in purus eu magna vulputate luctus.');
insert into CREW (CREW_ID, NAAM, GEBOORTEDATUM, GESLACHT, BIO) values (6, 'Irene Peterson', '1963-03-16', 1, 'Nulla neque libero, convallis eget, eleifend luctus, ultricies eu, nibh. Quisque id justo sit amet sapien dignissim vestibulum. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Nulla dapibus dolor vel est.');
insert into CREW (CREW_ID, NAAM, GEBOORTEDATUM, GESLACHT, BIO) values (7, 'Margaret Riley', '1961-10-08', 1, 'Duis at velit eu est congue elementum. In hac habitasse platea dictumst. Morbi vestibulum, velit id pretium iaculis, diam erat fermentum justo, nec condimentum neque sapien placerat ante. Nulla justo. Aliquam quis turpis eget elit sodales scelerisque. Mauris sit amet eros.');
insert into CREW (CREW_ID, NAAM, GEBOORTEDATUM, GESLACHT, BIO) values (8, 'Louis Wilson', '1985-03-09', 0, 'Pellentesque eget nunc. Donec quis orci eget orci vehicula condimentum. Curabitur in libero ut massa volutpat convallis. Morbi odio odio, elementum eu, interdum eu, tincidunt in, leo. Maecenas pulvinar lobortis est. Phasellus sit amet erat. Nulla tempus. Vivamus in felis eu sapien cursus vestibulum.');
insert into CREW (CREW_ID, NAAM, GEBOORTEDATUM, GESLACHT, BIO) values (9, 'Phyllis Long', '1991-02-21', 1, 'Nullam molestie nibh in lectus. Pellentesque at nulla. Suspendisse potenti. Cras in purus eu magna vulputate luctus. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.');
insert into CREW (CREW_ID, NAAM, GEBOORTEDATUM, GESLACHT, BIO) values (10, 'Julie Franklin', '1968-10-30', 1, 'Vivamus vel nulla eget eros elementum pellentesque. Quisque porta volutpat erat. Quisque erat eros, viverra eget, congue eget, semper rutrum, nulla. Nunc purus. Phasellus in felis. Donec semper sapien a libero. Nam dui. Proin leo odio, porttitor id, consequat in, consequat ut, nulla. Sed accumsan felis.');

-- CREW_TYPE

insert into CREW_TYPE (TYPE_ID, NAAM) values (1, 'Director');
insert into CREW_TYPE (TYPE_ID, NAAM) values (2, 'Acteur');
insert into CREW_TYPE (TYPE_ID, NAAM) values (3, 'Camera');

-- GENRE

insert into GENRE (GENRE_ID, NAAM) values (1, 'Actie');
insert into GENRE (GENRE_ID, NAAM) values (2, 'Animatie');

-- FILM

insert into FILM (FILM_ID, TITEL, DUUR, `release`, GESPROKEN_TAAL, ONDERTITEL_TAAL, GENRE_ID) values (1, 'The Huntsman: Winters War', 114, '2016-04-07', 'en', 'nl', 1);
insert into FILM (FILM_ID, TITEL, DUUR, `release`, GESPROKEN_TAAL, GENRE_ID) values (2, 'Kung Fu Panda 3', 96, '2016-04-13', 'nl', 2);
insert into FILM (FILM_ID, TITEL, DUUR, `release`, GESPROKEN_TAAL, ONDERTITEL_TAAL, GENRE_ID) values (3, 'Batman v Superman: Dawn of Justice', 153, '2016-03-24', 'en', 'nl', 1);
insert into FILM (FILM_ID, TITEL, DUUR, `release`, GESPROKEN_TAAL, GENRE_ID, SEQUEL_TO) values (4, 'Kung Fu Panda 4', 98, '2017-09-27', 'nl', 2, 2);

-- FILM_CREW_MAP

insert into FILM_CREW_MAP (FILM_ID, CREW_ID, TYPE_ID) values (1, 1, 1);
insert into FILM_CREW_MAP (FILM_ID, CREW_ID, TYPE_ID) values (1, 2, 2);
insert into FILM_CREW_MAP (FILM_ID, CREW_ID, TYPE_ID) values (1, 3, 3);
insert into FILM_CREW_MAP (FILM_ID, CREW_ID, TYPE_ID) values (2, 1, 1);
insert into FILM_CREW_MAP (FILM_ID, CREW_ID, TYPE_ID) values (2, 4, 2);
insert into FILM_CREW_MAP (FILM_ID, CREW_ID, TYPE_ID) values (2, 5, 2);
insert into FILM_CREW_MAP (FILM_ID, CREW_ID, TYPE_ID) values (3, 7, 1);
insert into FILM_CREW_MAP (FILM_ID, CREW_ID, TYPE_ID) values (3, 7, 2);
insert into FILM_CREW_MAP (FILM_ID, CREW_ID, TYPE_ID) values (3, 9, 3);

-- BIOSCOOP

insert into BIOSCOOP (BIOSCOOP_ID, NAAM, STRAAT, HUISNUMMER, POSTCODE, PLAATS) values (1, 'Schouwburgplein', 'Schouwburgplein', 101, '3012CL', 'Rotterdam');
insert into BIOSCOOP (BIOSCOOP_ID, NAAM, STRAAT, HUISNUMMER, POSTCODE, PLAATS) values (2, 'De Kuip', 'Cor Kieboomplein', 501, '3077MK', 'Rotterdam');
insert into BIOSCOOP (BIOSCOOP_ID, NAAM, STRAAT, HUISNUMMER, POSTCODE, PLAATS) values (3, 'Tilburg', 'Pieter Vreedeplein', 174, '5038BW', 'Tilburg');

-- ZAAL

insert into ZAAL (ZAAL_ID, NUMMER, BIOSCOOP_ID) values (1, 1, 1);
insert into ZAAL (ZAAL_ID, NUMMER, BIOSCOOP_ID) values (2, 2, 1);
insert into ZAAL (ZAAL_ID, NUMMER, BIOSCOOP_ID) values (3, 3, 1);
insert into ZAAL (ZAAL_ID, NUMMER, BIOSCOOP_ID) values (4, 1, 2);
insert into ZAAL (ZAAL_ID, NUMMER, BIOSCOOP_ID) values (5, 2, 2);
insert into ZAAL (ZAAL_ID, NUMMER, BIOSCOOP_ID) values (6, 3, 2);
insert into ZAAL (ZAAL_ID, NUMMER, BIOSCOOP_ID) values (7, 1, 3);
insert into ZAAL (ZAAL_ID, NUMMER, BIOSCOOP_ID) values (8, 2, 3);
insert into ZAAL (ZAAL_ID, NUMMER, BIOSCOOP_ID) values (9, 3, 3);

-- EVENT

insert into EVENT (EVENT_ID, NAAM, BEGIN_DATUM, EIND_DATUM) values (1, 'Marathon', '2016-04-10', '2016-04-10');
insert into EVENT (EVENT_ID, NAAM, BEGIN_DATUM, EIND_DATUM) values (2, 'Yet Another Marathon', '2016-05-01', '2016-05-02');
insert into EVENT (EVENT_ID, NAAM, BEGIN_DATUM, EIND_DATUM) values (3, 'Leeg Event', '2016-04-21', '2016-04-21');

-- VOORSTELLING

insert into VOORSTELLING (VOORSTELLING_ID, FILM_ID, ZAAL_ID, DATUM) values (1, 1, 1, '2016-04-10 13:15');
insert into VOORSTELLING (VOORSTELLING_ID, FILM_ID, ZAAL_ID, DATUM) values (2, 1, 2, '2016-04-10 16:00');
insert into VOORSTELLING (VOORSTELLING_ID, FILM_ID, ZAAL_ID, DATUM) values (3, 2, 1, '2016-04-11 8:45');
insert into VOORSTELLING (VOORSTELLING_ID, FILM_ID, ZAAL_ID, DATUM) values (4, 2, 7, '2016-04-10 12:00');
insert into VOORSTELLING (VOORSTELLING_ID, FILM_ID, ZAAL_ID, DATUM) values (5, 1, 8, '2016-04-11 19:20');
insert into VOORSTELLING (VOORSTELLING_ID, FILM_ID, ZAAL_ID, DATUM) values (6, 1, 9, '2016-04-10 10:40');
insert into VOORSTELLING (VOORSTELLING_ID, FILM_ID, ZAAL_ID, EVENT_ID, DATUM) values (7, 1, 5, 1, '2016-04-10 09:00');
insert into VOORSTELLING (VOORSTELLING_ID, FILM_ID, ZAAL_ID, EVENT_ID, DATUM) values (8, 2, 5, 1, '2016-04-10 15:05');
insert into VOORSTELLING (VOORSTELLING_ID, FILM_ID, ZAAL_ID, EVENT_ID, DATUM) values (9, 2, 2, 2, '2016-05-01 15:45');
insert into VOORSTELLING (VOORSTELLING_ID, FILM_ID, ZAAL_ID, EVENT_ID, DATUM) values (10, 3, 2, 2, '2016-05-02 16:15');

-- GEBRUIKER

insert into GEBRUIKER (EMAIL, NAAM, WACHTWOORD, ADMIN) values ('sven.dubbeld1@gmail.com', 'Sven Dubbeld', 'Wachtwoord', 0);
insert into GEBRUIKER (EMAIL, NAAM, WACHTWOORD, ADMIN) values ('sven@svendubbeld.nl', 'S. Dubbeld', 'Wachtwoord', 1);
insert into GEBRUIKER (EMAIL, NAAM, WACHTWOORD, ADMIN) values ('s.dubbeld@student.fontys.nl', 'Dubbeld, S.', 'Wachtwoord', 0);
insert into GEBRUIKER (EMAIL, NAAM, WACHTWOORD, ADMIN) values ('rwright0@tamu.edu', 'Ryan Wright', 'qiM9TSpzsb', 1);
insert into GEBRUIKER (EMAIL, NAAM, WACHTWOORD, ADMIN) values ('daustin1@sakura.ne.jp', 'Denise Austin', 'y0F6mSfCvd', 1);
insert into GEBRUIKER (EMAIL, NAAM, WACHTWOORD, ADMIN) values ('rschmidt2@furl.net', 'An Schmidt', 'pXnHQdfuuYv', 1);
insert into GEBRUIKER (EMAIL, NAAM, WACHTWOORD, ADMIN) values ('rchavez3@patch.com', 'Rachel Chavez', 'yfRtz35gci', 1);
insert into GEBRUIKER (EMAIL, NAAM, WACHTWOORD, ADMIN) values ('astone4@who.int', 'Alice Stone', '0Tr4nYBZ', 1);

-- BESTELLING

insert into BESTELLING (BESTELNR, USER_ID, DATUM, PRIJS) values (1, 1, '2016-04-09', 3600);
insert into BESTELLING (BESTELNR, USER_ID, DATUM, PRIJS) values (2, 1, '2016-04-09', 1200);
insert into BESTELLING (BESTELNR, USER_ID, DATUM, PRIJS) values (3, 2, '2016-04-09', 3600);

-- STOEL

insert into STOEL (STOEL_ID, NUMMER, RIJ, ZAAL_ID) values (1, 1, 1, 1);
insert into STOEL (STOEL_ID, NUMMER, RIJ, ZAAL_ID) values (2, 2, 1, 1);
insert into STOEL (STOEL_ID, NUMMER, RIJ, ZAAL_ID) values (3, 3, 1, 1);
insert into STOEL (STOEL_ID, NUMMER, RIJ, ZAAL_ID) values (4, 1, 1, 2);

-- KAARTJE

insert into KAARTJE (VOORSTELLING_ID, STOEL_ID, BESTELLING_ID) values (1, 1, 1);
insert into KAARTJE (VOORSTELLING_ID, STOEL_ID, BESTELLING_ID) values (1, 2, 1);
insert into KAARTJE (VOORSTELLING_ID, STOEL_ID, BESTELLING_ID) values (1, 3, 1);
insert into KAARTJE (VOORSTELLING_ID, STOEL_ID, BESTELLING_ID) values (2, 1, 2);
insert into KAARTJE (VOORSTELLING_ID, STOEL_ID, BESTELLING_ID) values (2, 2, 3);
insert into KAARTJE (VOORSTELLING_ID, STOEL_ID, BESTELLING_ID) values (1, 4, 3);
insert into KAARTJE (VOORSTELLING_ID, STOEL_ID, BESTELLING_ID) values (3, 1, 3);

-- MEDIA_ITEM

insert into MEDIA_ITEM (ITEM_ID, FILM_ID, NAAM, OMSCHRIJVING, URL, TYPE) values (1, 1, 'Trailer', 'De trailert', 'https://youtube.com/a', 'youtube');
insert into MEDIA_ITEM (ITEM_ID, FILM_ID, NAAM, OMSCHRIJVING, URL, TYPE) values (2, 3, 'Teaser', 'De trailer teaser', 'https://youtube.com/b', 'youtube');
insert into MEDIA_ITEM (ITEM_ID, FILM_ID, NAAM, OMSCHRIJVING, URL, TYPE) values (3, 4, 'Trailer', 'De trailert', 'https://youtube.com/c', 'youtube');
insert into MEDIA_ITEM (ITEM_ID, FILM_ID, NAAM, OMSCHRIJVING, URL, TYPE) values (4, 4, 'Teaser', 'De trailer treaser', 'https://youtube.com/d', 'youtube');
insert into MEDIA_ITEM (ITEM_ID, FILM_ID, NAAM, OMSCHRIJVING, URL, TYPE) values (5, 1, 'Cast Interview', 'Interview met de cast', 'https://youtube.com/e', 'youtube');

-- MEDIA_ITEM_SUGGESTION

insert into MEDIA_ITEM_SUGGESTION (FIRST_ITEM_ID, SECOND_ITEM_ID) values (1, 2);
insert into MEDIA_ITEM_SUGGESTION (FIRST_ITEM_ID, SECOND_ITEM_ID) values (5, 3);
insert into MEDIA_ITEM_SUGGESTION (FIRST_ITEM_ID, SECOND_ITEM_ID) values (4, 1);

-- BLOG

insert into BLOG (BLOG_ID, TITEL, DATUM, BODY, AUTEUR) values (1, 'Etiam faucibus cursus urna.', '2015-10-28', 'In tempor, turpis nec euismod scelerisque, quam turpis adipiscing lorem, vitae mattis nibh ligula nec sem. Duis aliquam convallis nunc. Proin at turpis a pede posuere nonummy. Integer non velit. Donec diam neque, vestibulum eget, vulputate ut, ultrices vel, augue. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Donec pharetra, magna vestibulum aliquet ultrices, erat tortor sollicitudin mi, sit amet lobortis sapien sapien non mi. Integer ac neque. Duis bibendum. Morbi non quam nec dui luctus rutrum. Nulla tellus.', 5);
insert into BLOG (BLOG_ID, TITEL, DATUM, BODY, AUTEUR) values (2, 'Proin at turpis a pede posuere nonummy.', '2015-11-13', 'Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Etiam vel augue. Vestibulum rutrum rutrum neque. Aenean auctor gravida sem. Praesent id massa id nisl venenatis lacinia. Aenean sit amet justo.', 5);
insert into BLOG (BLOG_ID, TITEL, DATUM, BODY, AUTEUR) values (3, 'Nullam varius.', '2015-12-05', 'Morbi ut odio. Cras mi pede, malesuada in, imperdiet et, commodo vulputate, justo. In blandit ultrices enim. Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Proin interdum mauris non ligula pellentesque ultrices. Phasellus id sapien in sapien iaculis congue. Vivamus metus arcu, adipiscing molestie, hendrerit at, vulputate vitae, nisl.', 8);
insert into BLOG (BLOG_ID, TITEL, DATUM, BODY, AUTEUR) values (4, 'Donec quis orci eget orci vehicula condimentum.', '2015-06-24', 'Morbi a ipsum. Integer a nibh. In quis justo. Maecenas rhoncus aliquam lacus. Morbi quis tortor id nulla ultrices aliquet. Maecenas leo odio, condimentum id, luctus nec, molestie sed, justo. Pellentesque viverra pede ac diam.', 6);
insert into BLOG (BLOG_ID, TITEL, DATUM, BODY, AUTEUR) values (5, 'Vivamus metus arcu, adipiscing molestie, hendrerit at, vulputate vitae, nisl.', '2015-07-04', 'Proin eu mi. Nulla ac enim. In tempor, turpis nec euismod scelerisque, quam turpis adipiscing lorem, vitae mattis nibh ligula nec sem. Duis aliquam convallis nunc. Proin at turpis a pede posuere nonummy. Integer non velit.', 8);