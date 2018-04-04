# Custom Registration fields per tenant

One of my clients had multiple upcoming projects that need custom registration fields. The goal here was to make something reusable and configurable so that per tenant there can be easily configured custom registration fields. 

### The solution provides the following

1. A configuration ([app-userproperties.json](https://github.com/joeaudette/cloudscribe.Entropy/blob/master/solutions/cloudscribe.CustomRegistration.Kvp/src/WebApp/app-userproperties.json)) based way to define custom fields per tenant for the registration page, manage user info page, and admin user edit pages so that the captured custom data can be viewed and optionally editable. It supports validation requirements per field currently supporting input, select, textarea, datepicker, and dateofbirthpicker, and read only label views. You can define custom views per property or choose from the ones already included. 

2. The captured custom data is stored in a generic key value data store that could also be reused for other purposes.

3. You can add custom form fields automatically to views based on the config file without having to manually manage all the form elements in the views. We could also if needed make custom views per tenant because of the ability to add custom views per tenant already provided in cloudscribe Core. See how we are adding custom form fields in [RegisterMiddle](https://github.com/joeaudette/cloudscribe.Entropy/blob/master/solutions/cloudscribe.CustomRegistration.Kvp/src/WebApp/Views/Account/RegisterMiddle.cshtml), [UserInfoMiddle](https://github.com/joeaudette/cloudscribe.Entropy/blob/master/solutions/cloudscribe.CustomRegistration.Kvp/src/WebApp/Views/Manage/UserInfoMiddlePartial.cshtml), and [UserEditMiddle](https://github.com/joeaudette/cloudscribe.Entropy/blob/master/solutions/cloudscribe.CustomRegistration.Kvp/src/WebApp/Views/UserAdmin/UserEditMiddlePartial.cshtml) views with very little code.

4. Custom implementations of [IHandleCustomRegistration](https://github.com/joeaudette/cloudscribe.Entropy/blob/master/solutions/cloudscribe.CustomRegistration.Kvp/src/cloudscribe.UserProperties.Web.Kvp/KvpRegistrationHandler.cs), [IHandleCustomUserInfo](https://github.com/joeaudette/cloudscribe.Entropy/blob/master/solutions/cloudscribe.CustomRegistration.Kvp/src/cloudscribe.UserProperties.Web.Kvp/KvpUserInfoHandler.cs), and [IHandleCustomUserInfoAdmin](https://github.com/joeaudette/cloudscribe.Entropy/blob/master/solutions/cloudscribe.CustomRegistration.Kvp/src/cloudscribe.UserProperties.Web.Kvp/KvpUserInfoAdminHandler.cs) interfaces from cloudscribe Core process the additional form fields

### Credits

![esdm logo](https://www.cloudscribe.com/media/images/esdm-banner.png)

The code in this solution was developed under sponsored open source development with funding provided by [exeGesIS Spatial Data Management](https://www.esdm.co.uk/)

If you are interested in sponsoring additional open source features for the cloudscribe ecosystem, please [contact me](https://www.cloudscribe.com/contact)

See the [complete list of cloudscribe libraries](https://www.cloudscribe.com/docs/complete-list-of-cloudscribe-libraries) on [cloudscribe.com](https://www.cloudscribe.com/)


[![Join the chat at https://gitter.im/joeaudette/cloudscribe](https://badges.gitter.im/joeaudette/cloudscribe.svg)](https://gitter.im/joeaudette/cloudscribe?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)




