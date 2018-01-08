# spring-net-json-variable-source

As resharper does not get along with the default properties files for spring and because we are used to configure things in json format, we decided to use a json-variable-source for spring.net.

This comes with some very positiv side effects:
- Validation of the (json)-config
- Intellisense in Visual Studio when writing the json-file
- complex configuration objects
- No more Resharper errors when opening the properties file
- maybe more ...


This is a sample the json-config-file ...


![json-config](readme/json.png)


... for the specified json-schema ...


![json-schema-file](readme/schema.png)


With intellisense ...


![json-schema-file](readme/intellisense.png)


... and validation ...
... for missing properties ...


![json-schema-file](readme/validation1.png)


![json-schema-file](readme/validation2.png)


... for invalid types ...


![Validation of type](readme/validation3.png)