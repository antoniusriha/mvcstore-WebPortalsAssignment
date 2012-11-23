-- Membership administrator role and user setup for MvcStore

INSERT INTO "Roles" (
	"Rolename",
	"ApplicationName")
VALUES (
	'Administrator',
	'MvcStore'
);

INSERT INTO "Users" (
	"pId",
	"Username",
	"ApplicationName",
	"Email",
	"Comment",
	"Password",
	"PasswordQuestion",
	"PasswordAnswer",
	"IsApproved",
	"LastActivityDate",
	"LastLoginDate",
	"LastPasswordChangedDate",
	"CreationDate",
	"IsOnLine",
	"IsLockedOut",
	"LastLockedOutDate",
	"FailedPasswordAttemptCount",
	"FailedPasswordAttemptWindowStart",
	"FailedPasswordAnswerAttemptCount",
	"FailedPasswordAnswerAttemptWindowStart")
VALUES (
	'937c1a49-aa14-43b9-b3c9-3f1172e8f6a8',
	'Administrator',
	'MvcStore',
	'antoniusriha@gmail.com',
	NULL,
	'PwTDdviu1N/829M29Hq8etRG0lA=',
	'question',
	'vaKgPWnm8iiM6z5agLNsoO7Drsc=',
	TRUE,
	'2012-11-22 19:17:30.996944+01',
	NULL,
	'2012-11-22 19:17:30.996944+01',
	'2012-11-22 19:17:30.996944+01',
	NULL,
	FALSE,
	'2012-11-22 19:17:30.996944+01',
	'0',
	'2012-11-22 19:17:30.996944+01',
	'0',
	'2012-11-22 19:17:30.996944+01'
);

INSERT INTO "UsersInRoles" (
	"Username",
	"Rolename",
	"ApplicationName")
VALUES (
	'Administrator',
	'Administrator',
	'MvcStore'
);
