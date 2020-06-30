USE master;
GO

-- Delete the DemoDB Database (IF EXISTS)
IF EXISTS(select * from sys.databases where name='CityTours')
DROP DATABASE CityTours;
GO

-- Create a new DemoDB Database
CREATE DATABASE CityTours;
GO

USE [CityTours]
GO
/****** Object:  Table [dbo].[landmark]    Script Date: 4/8/2020 9:36:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[landmark](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[longName] [varchar](100) NOT NULL,
	[shortName] [varchar](100) NULL,
	[streetNumber] [int] NOT NULL,
	[streetName] [varchar](25) NOT NULL,
	[city] [varchar](50) NOT NULL,
	[state] [varchar](20) NOT NULL,
	[stateCode] [varchar](20) NOT NULL,
	[zip] [int] NOT NULL,
	[latitude] [decimal](18, 4) NOT NULL,
	[longitude] [decimal](18, 4) NOT NULL,
	[details] [varchar](2000) NOT NULL,
	[isApproved] [bit] NOT NULL
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[landmark] ON 

INSERT [dbo].[landmark] ([id], [longName], [shortName], [streetNumber], [streetName], [city], [state], [stateCode], [zip], [latitude], [longitude], [details], [isApproved]) VALUES (1, N'Statue Of Liberty', NULL, 1, N' Battery Place', N'New York City', N'New York', N'NY', 10004, CAST(40.6892 AS Decimal(18, 4)), CAST(-74.0445 AS Decimal(18, 4)), N'Presented to the United States in 1886 as a gift from France, Lady Liberty is a near-universal symbol of freedom and democracy, standing 305 feet and 6 inches high on Liberty Island. You can get a sense of the thrill millions of immigrants must have experienced as you approach it on the ferry from Battery Park and see the statue grow from a vaguely defined figure on the horizon into a towering, stately colossus. Here’s Fodor’s Guide to all things New York City.
', 1)
INSERT [dbo].[landmark] ([id], [longName], [shortName], [streetNumber], [streetName], [city], [state], [stateCode], [zip], [latitude], [longitude], [details], [isApproved]) VALUES (2, N'Hoover Dam', NULL, 1305, N'Arizona Street
', N'Boulder City', N'Nevada', N'NV', 89005, CAST(36.0161 AS Decimal(18, 4)), CAST(-114.7377 AS Decimal(18, 4)), N'Holding back the mighty Colorado River, this massive feat of engineering creates hydroelectric power and helps provides water for seven states and a portion of Mexico. In 2010, the Hoover Dam Bypass Bridge opened to allow for faster travel through the area. But it’s still worth stopping to admire the Art Deco wonder and tour the facilities.
', 1)
INSERT [dbo].[landmark] ([id], [longName], [shortName], [streetNumber], [streetName], [city], [state], [stateCode], [zip], [latitude], [longitude], [details], [isApproved]) VALUES (3, N'Gateway Arch', NULL, 11, N' N 4th St', N'St. Louis', N'Missouri', N'MO', 63102, CAST(38.6247 AS Decimal(18, 4)), CAST(-90.1848 AS Decimal(18, 4)), N'Part of the Gateway Arch National Park, this iconic structure symbolizes the importance of St. Louis as the Gateway to the West. Be sure to ride to the top for great views of the city and the Mississippi River.', 1)
INSERT [dbo].[landmark] ([id], [longName], [shortName], [streetNumber], [streetName], [city], [state], [stateCode], [zip], [latitude], [longitude], [details], [isApproved]) VALUES (5, N'Golden Gate Bridge', NULL, 1, N'Golden Gate Bridge', N'San Francisco', N'California', N'CA', 62843, CAST(37.8199 AS Decimal(18, 4)), CAST(-122.4783 AS Decimal(18, 4)), N'The suspension bridge connecting San Francisco with Marin County, completed in 1937, is a triumph in just about every way. With its 1.7-mi span and 746-foot towers, it’s both beautiful and durable—it was built to withstand winds of more than 100 mph and was undamaged by the 1989 Loma Prieta quake. The bridge’s walkway provides unparalleled views of the Bay Area.', 1)
INSERT [dbo].[landmark] ([id], [longName], [shortName], [streetNumber], [streetName], [city], [state], [stateCode], [zip], [latitude], [longitude], [details], [isApproved]) VALUES (6, N'Mount Rushmore', NULL, 13000, N' SD-244', N'Keystone', N'South Dakota', N'SD', 57751, CAST(43.8791 AS Decimal(18, 4)), CAST(-103.4591 AS Decimal(18, 4)), N'In the midst of South Dakota’s Black Hills, 60-foot-high likenesses of Presidents George Washington, Thomas Jefferson, Abraham Lincoln, and Theodore Roosevelt are carved into a massive granite cliff; the result is America’s most famous memorial. From sunset through 9 pm, the majestic faces are dramatically illuminated at night.
', 1)
SET IDENTITY_INSERT [dbo].[landmark] OFF


CREATE TABLE itinerary (
id int identity Primary Key NOT NULL,
ownerID int NOT NULL,
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

CREATE TABLE landmarkReview(
id int identity Primary Key,
rating decimal(5,2) NOT NULL,
title varchar(50) NOT NULL,
body varchar(400) NOT NULL,
landmarkId int NOT NULL,
userId int NOT NULL,
upvoteCount int default(0),
downvoteCount int default(0)
)

CREATE TABLE itineraryReview(
id int identity Primary Key,
rating decimal(5,2) NOT NULL,
title varchar(50) NOT NULL,
body varchar(400) NOT NULL,
itineraryId int NOT NULL,
userId int NOT NULL,
upvoteCount int default(0),
downvoteCount int default(0)
)

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

ALTER TABLE landmarkReview
WITH CHECK ADD CONSTRAINT FK_landmarkReview_landmark
FOREIGN KEY (landmarkId)
REFERENCES landmark(id)

ALTER TABLE landmarkReview
WITH CHECK ADD CONSTRAINT FK_landmarkReview_user
FOREIGN KEY (userId)
REFERENCES [user](id)

ALTER TABLE itineraryReview
WITH CHECK ADD CONSTRAINT FK_itineraryReview_itinerary
FOREIGN KEY (itineraryId)
REFERENCES itinerary(id)

ALTER TABLE itineraryReview
WITH CHECK ADD CONSTRAINT FK_itineraryReview_user
FOREIGN KEY (userId)
REFERENCES [user](id)


INSERT INTO itinerary ([name], startingLatitude, startingLongitude, ownerID)
VALUES ('Tour of Downtown Columbus', 40.0389, -82.8748, 2), ('Educational Tour', 40.0389, -82.8748, 2)

INSERT INTO [user](username, password, salt, role)
VALUES ('user', '/kqaY86Ou3/DR9oWe8LZDL/Chvc=', 'FosijN09SxE=','user'), ('admin', 'g0p6eM7r5DrKr5nSpNJeD5Aa+qY=', 'c5wa2T/Tx2I=', 'admin')

INSERT INTO landmark (longName, streetNumber,streetName,city,[state], stateCode,zip,latitude,longitude,details,isApproved)
VALUES ('Kroger Bakery', 457, 'Cleveland Avenue', 'Columbus', 'Ohio','OH',43215, 39.9736,-82.9910, 'The Ford Motor Company built this substantial brick factory with ornamental white terra cotta trim across Cleveland Avenue from Fort Hayes. Designed by Ford Architect John Graham, Model T parts arrived by train and were assembled here and distributed to area dealerships until c1925. Assembly at this site ended in 1932 and the plant closed in 1939. The building was later adapted as an expansion of the neighboring commercial bakery for the Kroger Company where 14 production lines filled downtown Columbus with a sweet aroma for 90+ years. The Kroger Bakery closed in February 2019, citing outdated layout of the plant and aging equipment, and eliminating 411 jobs.',1)

INSERT INTO landmark (longName, shortName, streetNumber,streetName,city,[state], stateCode,zip,latitude,longitude,details,isApproved)
VALUES('Franklin Park Conservatory and Botanical Gardens', 'Franklin Park Conservatory', 1777,'E. Broad Street','Columbus', 'Ohio', 'OH', 43203,39.965330,-82.954540,'A botanical landmark just two miles east of downtown Columbus, Franklin Park Conservatory and Botanical Gardens features exotic plant collections and displays, seasonal exhibitions, outdoor gardens (including community and culinary gardens) and a variety of educational programming. All set within the 88-acre Franklin Park. Inspired by horticulture, Franklin Park Conservatory and Botanical Gardens elevates quality of life and connects the community through educational, cultural and social experiences. All ages can partake in a variety of special events and activities throughout the year. A full menu of classes, workshops and camps are offered in gardening, cooking, fine art and wellness. With its natural surroundings, both indoors and out, the Conservatory serves as a premier venue for special events and corporate gatherings.',1)

INSERT INTO landmark (longName, streetNumber,streetName,city,[state], stateCode,zip,latitude,longitude,details,isApproved)
VALUES ('Ohio Stadium', 410 ,'Woody Hayes Dr','Columbus','Ohio','OH',43210,40.005220,-83.019110,'Nestled snugly on the banks of the Olentangy River, stately Ohio Stadium is one of the most recognizable landmarks in all of college athletics. With its present seating capacity of 102,780, Ohio Stadium is the fourth-largest on-campus facility in the nation. Since the opening game against Ohio Wesleyan on Oct. 7, 1922, more than 36 million fans have streamed through the stadium’s portals.',1)

INSERT INTO landmark (longName, streetNumber,streetName,city,[state], stateCode,zip,latitude,longitude,details,isApproved)
VALUES ('Columbus Museum of Art', 480, ' E Broad St','Columbus','Ohio','OH',43215,39.964821,-82.978752, 'Columbus Museum of Art’s mission is to create great experiences with great art for everyone. Whether we are presenting an exhibition, designing an art-making activity, or giving visitors directions, we are guided by a vision to connect people and art. CMA nurtures that connection and removes barriers between our community and our collection. There’s a willingness at CMA to try new things. We encourage curiosity about art, conversations about creativity, and connections with cultures. A community hub where ideas can be exchanged and different voices heard, the Museum nurtures creativity through building relationships with diverse partners and designing engaging experiences.',1)

INSERT INTO landmark (longName, streetNumber,streetName,city,[state], stateCode,zip,latitude,longitude,details,isApproved)
VALUES ('German Village', 588, 'S 3rd St', 'Columbus', 'Ohio','OH',43215,39.950380,-82.995000,'A highway bridge over Interstate 70 is all that separates the German Village Historic District from downtown Columbus, but as one looks east from the interstate, the difference between old and new is glaring. A 20+ story structure sits just north of the interstate bridge, and just south, in German Village, no structure is higher than three stories. Five blocks south, the spire of St. Mary Church stands 197’ off the sidewalk and towers over everything around it. Structures and sidewalks are orange masonry, and many streets (about half) are still brick pavers. German Village does not have a recreated sense of history or kitschy Bavarian feel ~ rather, it is a neighborhood with architecture dating from the 1840s-1890s that has been preserved, and its use as a shared residential and commercial neighborhood has been maintained. People walk to their destinations, park on the street due to the overwhelming absence of driveways, and live life at a very pedestrian level. The neighborhood is extremely dense ~ very often only inches separate neighboring structures, and many structures were built for multi-family use. German Village is notably different because its appearance has changed so little.', 0)

INSERT INTO landmark_itinerary(landmarkId, itineraryId)
VALUES ((SELECT id FROM landmark WHERE id=11),(SELECT id FROM itinerary WHERE [name]='Tour of Downtown Columbus')),
((SELECT id FROM landmark WHERE id=7),(SELECT id FROM itinerary WHERE [name]='Tour of Downtown Columbus')),
((SELECT id FROM landmark WHERE id=9),(SELECT id FROM itinerary WHERE [name]='Tour of Downtown Columbus')),
((SELECT id FROM landmark WHERE id=8),(SELECT id FROM itinerary WHERE [name]='Educational Tour')),
((SELECT id FROM landmark WHERE id=10),(SELECT id FROM itinerary WHERE [name]='Educational Tour'))

INSERT INTO user_itinerary(userId, itineraryId)
VALUES ((SELECT id FROM [user] WHERE username = 'admin'),(SELECT id FROM itinerary WHERE [name]='Tour of Downtown Columbus')),
((SELECT id FROM [user] WHERE username='admin'),(SELECT id FROM itinerary WHERE [name]='Educational Tour'))

INSERT INTO landmarkReview (rating, title, body, landmarkId, userID)
VALUES (2.5, 'This place sucks', 'This place really sucks', 1, 1),(3, 'This place does not suck', 'This place really does not suck', 8, 2),
(5, 'This place is great', 'This place is really great', 3, 2),(1, 'Will never go back', 'This place really really sucks', 1, 1),
(5, 'I want to move here', 'This place is my favorite place on earth', 7, 2)

INSERT INTO itineraryReview (rating, title, body, itineraryId, userID)
VALUES (2.5, 'This itinerary sucks', 'This itinerary really sucks', 1, 1),(3, 'This itinerary does not suck', 'This itinerary really does not suck', 1, 2),
(5, 'This itinerary is great', 'This itinerary is really great', 2, 1),(1, 'Will never use this itinerary', 'This itinerary really really sucks', 1, 1),
(5, 'I want to marry this itinerary', 'This itinerary is my favorite thing on earth', 1, 2)
