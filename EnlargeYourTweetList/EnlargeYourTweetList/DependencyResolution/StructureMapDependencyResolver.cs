    // --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureMapDependencyResolver.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Web.Mvc;
using EnlargeYourTweetList.Model.MongoDB;
using EnlargeYourTweetList.Model.Users;
using MongoDB.Driver;
using StructureMap;
using StructureMap.Pipeline;

namespace EnlargeYourTweetList.DependencyResolution
{
    public class StructureMapControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext,
                                                             Type controllerType)
        {
            try
            {
                if ((requestContext == null) || (controllerType == null))
                    return null;

                return (Controller) ObjectFactory.GetInstance(controllerType);
            }
            catch (StructureMapException)
            {
                System.Diagnostics.Debug.WriteLine(ObjectFactory.WhatDoIHave());
                throw new Exception(ObjectFactory.WhatDoIHave());
            }
        }
    }

    public static class RegisterDependency
    {
        public static void Run()
        {
            ControllerBuilder.Current.SetControllerFactory(new StructureMapControllerFactory());
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString;

            var databaseName = MongoUrl.Create(connectionString).DatabaseName;
            var server = MongoServer.Create(connectionString);
            var db = server.GetDatabase(databaseName);

            ObjectFactory.Initialize(x =>
            {
                x.For<MongoRepository<User>>().LifecycleIs(new HybridLifecycle()).Use(
                  new MongoRepository<User>(db));
                //x.For<MongoRepository<Competition>>().LifecycleIs(new HybridLifecycle()).Use(
                //    new MongoRepository<Competition>(db));
                //x.For<MongoRepository<Result>>().LifecycleIs(new HybridLifecycle()).Use(
                //   new MongoRepository<Result>(db));
            });
        }

      
    }
}