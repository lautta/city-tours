BEGIN TRANSACTION

CREATE TABLE itinerary (
id int identity Primary Key NOT NULL,
name varchar(50) NOT NULL,
startingLatitude decimal(18,4) NOT NULL,
startingLongitude decimal(18,4) NOT NULL
);

CREATE TABLE [user]
(
	id			int			identity(1,1),
	username	varchar(50)	not null,
	password	varchar(50)	not null,
	salt		varchar(50)	not null,
	role		varchar(50)	default('user'),

	constraint pk_users primary key (id)
);

CREATE TABLE user_itinerary(
userId int NOT NULL,
itineraryId int NOT NULL
);

CREATE TABLE landmark_itinerary(
landmarkId int NOT NULL,
itineraryId int NOT NULL
);

ALTER TABLE user_itinerary
WITH CHECK ADD CONSTRAINT FK_userItinerary_user
FOREIGN KEY (userId)
REFERENCES [user](id)

ALTER TABLE user_itinerary
WITH CHECK ADD CONSTRAINT FK_userItinerary_itinerary
FOREIGN KEY (itineraryId)
REFERENCES itinerary(id)

ALTER TABLE landmark_itinerary
WITH CHECK ADD CONSTRAINT FK_landmarkItinerary_landmark
FOREIGN KEY (landmarkId)
REFERENCES landmark(id)

ALTER TABLE landmark_itinerary
WITH CHECK ADD CONSTRAINT FK_landmarkItinerary_itinerary
FOREIGN KEY (itineraryId)
REFERENCES itinerary(id)

COMMIT