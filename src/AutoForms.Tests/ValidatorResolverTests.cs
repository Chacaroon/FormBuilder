﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoForms.Enums;
using AutoForms.Extensions;
using AutoForms.Models;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace AutoForms.Tests;

public class ValidatorResolverTests
{
    private IServiceProvider _serviceProvider;

    [SetUp]
    public void SetUp()
    {
        var services = new ServiceCollection()
            .AddAutoForms();

        _serviceProvider = services.BuildServiceProvider();
    }

    [Test]
    [Ignore("Validation processing will be implemented as the plugin later")]
    public void Resolve_StringPropertyWithValidators_ReturnsControlWithValidators()
    {
        // Arrange
        var formBuilder = _serviceProvider.GetRequiredService<FormBuilderFactory>()
            .CreateFormBuilder<TestClass>();

        // Act
        var formGroup = (formBuilder.Build() as FormGroup)!;

        // Assert
        var testPropertyControl = formGroup.Controls.First(x => x.Key == nameof(TestClass.TestProperty).FirstCharToLowerCase()).Value;
        var requiredPropertyControl = formGroup.Controls.First(x => x.Key == nameof(TestClass.RequiredProperty).FirstCharToLowerCase()).Value;

        Assert.AreEqual(2, testPropertyControl.Validators.Length);
        Assert.NotNull(testPropertyControl.Validators.FirstOrDefault(x => x.Type == ValidatorType.MinLength));
        Assert.NotNull(testPropertyControl.Validators.FirstOrDefault(x => x.Type == ValidatorType.MaxLength));

        Assert.NotNull(requiredPropertyControl.Validators.FirstOrDefault(x => x.Type == ValidatorType.Required));
    }

    #region TestData

    private class TestClass
    {
        [MinLength(5)]
        [MaxLength(10)]
        public string TestProperty { get; set; }

        [Required]
        public string RequiredProperty { get; set; }
    }

    #endregion
}
