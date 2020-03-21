# mail-utils

A C# console  utility to send email from the command line, including attachments. The use case that it was developed for was to email nightly log snippets from an EC2 processing server to a group of administrators so they could review the logs from their phones rather than having to start the server up, remote into it, and review the logs there. In this particular case the server only ran in the off-hours and was shut down in the early morning hours after the nightly processing job completed.

Note - this utility uses my command-line parser `appsettings` for option parsing and displaying  usage instruction. That is a DLL project also hosted in GitHub: https://github.com/aceeric/appsettings.

### Command line options and parameters

**-to recipients**

Mandatory. A comma-separated list of email recipients. Two forms are supported. The simple form is: user@domain.ext. E.g.: yourbuddy@aol.com. The complex form is: Display Name \<user@domain.ext\>. E.g. "Joe Smythe \<joe.smythe@domain.ext\>". The recipients in the comma-separated list can mix and match forms.

**-from sender**

Mandatory. A single sender in one of the forms supported for "To" addresses. This sender address will appear in the "from" address of the email.

**-body body**

Mandatory. The message body, in plain text.

**-subject text**

Optional. The message subject, in plain text.

**-attachment path**

Optional. The path to a file to include as an attachment.

**-server server**

Mandatory. The SMPT server URL. E.g. *smtp.1and1.com*.

**-port nnn**

Mandatory. The SMPT port number to use.

**-user userid**

Mandatory. A user ID that has an account on the specified SMPT server. This is likely to be the same as the "from" option (assuming you are using the utility to send mail on your own behalf.) But that is not a requirement.

**-pass password**

Mandatory. The password of the specified user having an account on the specified SMPT server.

**-ssltype auto|onconnect**

Optional. Allowed literals are `auto` and `onconnect`. If `auto`, then the SSL connection type is determined by the server. If `onconnect`, then the client attempts to initiate an SSL connection immediately, creating a secure channel within which to negotiate the rest of the SSL connection. If the server does not support this, then the connection fails. This is the most secure option because the entire handshake is encrypted. If `auto` is specified, the server may elect to perform part of the handshake in plain text.

**-encrypted**

Optional. If specified, then the user ID & password on the command line are taken to be encrypted values. (See the -encrypt option below.) The utility will decrypt the provided credentials from the command line when accessing the SMTP server. If not provided, then the user ID & password are interpreted as plain text values.

**-encrypt**

Optional. Encrypts the provided user ID and password. Displays the encrypted values to the console, and immediately exits with no further processing. This option is used to generate encrypted credentials to use subsequently with the utility.

**-log file|db|con**

Optional. Determines how the utility communicates errors, status, etc. If not supplied, then all output goes to the console. If `file` is specified, then the utility logs to a log file in the same directory that the utility is run from. The log file will be named `load-file.log`. If `db` is specified, then logging occurs to the database. If `con` is specified, then output goes to the console (same as if the option were omitted.) If logging to file or db is specified then the utility runs silently with no console output.

If db logging is specified, then the required logging components must be installed in the database. If the components are not installed and db logging is specified, then the utility will automatically fail over to file-based logging.

Note: the C# utilizing this option requires the inclusion of my logging DLL which is also in GitHub: https://github.com/aceeric/logger. This utility also includes DDL for setting up database logging.

**-loglevel err|warn|info**

Optional. Defines the logging level. `err` specifies that only errors will be reported. `warn` means errors and warnings, and `info` means all messages. The default is `info`.

**-jobid guid**

Optional. Defines a job ID for the logging subsystem. A GUID value is supplied in the canonical 8-4-4-4-12 form. If provided, then the logging subsystem is initialized with the provided GUID. The default behavior is for the logging subsystem to generate its own GUID. The idea behind this option is - loading a file  might be one step in a multi-step job. Logging to the database and identifying each step with a GUID allows one to tie together job steps executed across different tooling using the job GUID.

### An Example

Here is an example from a PowerShell script that emails a log snippet. The full script runs as the last step in a larger processing stream. It clips the current night's log segment out of the full log, and writes it to a file named *temp-log.txt* in the current working directory. Then it invokes this statement to mail the logs to all responsible parties:

```powershell
.\mail-utils -to "notta.person@gmail.com,also.not@xyz.edu,etc..." `
 -from "SYSTEM PROCESS <robot@mycorp.com>" `
 -body "See subject" `
 -subject "Nightly Logs are attached: please review" `
 -attachment .\temp-log.txt `
 -server smtp.zzzzz.com `
 -port 587 `
 -user 0xENCRYPTEDUSERGOESHERE `
 -pass 0xENCRYPTEDPASSWORDGOESHERE `
 -ssltype auto `
 -encrypted `
 -log db `
 -loglevel info
```

*Note: this utility - along with most of the C# in by repo - was created as part of a private project. The project completed and the sponsor was kind enough to grant me the right to host the code in my repo. Hence no commit history.*

