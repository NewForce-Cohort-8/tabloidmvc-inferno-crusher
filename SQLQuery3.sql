USE [TabloidMVC]
GO

INSERT INTO [Comment] ([Subject], [Content], [PostId], [UserProfileId], [CreateDateTime])
VALUES ('Admina Strator is not cool or good', 'Hey guys, anyone else think we should really boot this Admina Strator person from this app? They seem like a real DOOFUS!! LOL but fr fr who with me?', 1, 2, DATEADD(day, 1, '1/22/2024'));

INSERT INTO [Comment] ([Subject], [Content], [PostId], [UserProfileId], [CreateDateTime])
VALUES ('Hey what the heck man', 'Dude what the hell I let you join this app that I built and maintain and now you are trying to do some weird mutiny stuff? OK whatever actually, you are 0 threat to me because you probably are some weird incel that lives in your parents basement. Do your worst weirdo!!', 1, 1, DATEADD(day, 1, '1/23/2024'));
