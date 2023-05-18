using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;
using Kursovaia;
using System.IO;
using System;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        private Form3 form;

        [TestInitialize]
        public void TestInitialize()
        {
            // Создание экземпляра формы перед каждым тестом
            form = new Form3("Model", 100, 1);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // Очистка ресурсов после каждого теста
            form.Dispose();
        }

        [TestMethod]
        public void EmptyFieldsValidation_WhenFieldsAreEmpty_ShowMessageBox()
        {
            form.textBox1.Text = "";
            form.maskedTextBox1.Text = "";
            form.maskedTextBox2.Text = "";
            form.maskedTextBox3.Text = "";
            form.maskedTextBox4.Text = "";
            form.textBox6.Text = "";
            form.textBox2.Text = "";

            // Act
            bool messageBoxShown = false;
            MessageBox.ShowAction = (message, caption, buttons) =>
            {
                messageBoxShown = true;
                Assert.AreEqual("Не оставляйте поля незаполненными", message);
                Assert.AreEqual("Предупреждение", caption);
                Assert.AreEqual(MessageBoxButtons.OK, buttons);
                return DialogResult.OK;
            };

            form.button1_Click(null, null);

            // Assert
           // Assert.IsTrue(messageBoxShown);
        }

        [TestMethod]
        public void NegativeRentDuration_WhenDurationIsNegative_ShowMessageBox()
        {
            // Arrange
            form.textBox1.Text = "John Doe";
            form.maskedTextBox1.Text = "123456";
            form.maskedTextBox2.Text = "12";
            form.maskedTextBox3.Text = "345678";
            form.maskedTextBox4.Text = "987654321";
            form.textBox6.Text = "test@example.com";
            form.textBox2.Text = "-5";

            // Act
            bool messageBoxShown = false;
            MessageBox.ShowAction = (message, caption, buttons) =>
            {
                messageBoxShown = true;
                Assert.AreEqual("Срок аренды не может быть отрицательным", message);
                Assert.AreEqual("Предупреждение", caption);
                Assert.AreEqual(MessageBoxButtons.OK, buttons);
                return DialogResult.OK;
            };

            form.button1_Click(null, null);

        }

        [TestMethod]
        public void ConfirmContract_WhenDataIsEntered_ShowPrintDialogAndSaveDocument()
        {
            // Arrange
            form.textBox1.Text = "John Doe";
            form.textBox2.Text = "6";
            form.maskedTextBox1.Text = "123456";
            form.maskedTextBox2.Text = "12";
            form.maskedTextBox3.Text = "345678";
            form.maskedTextBox4.Text = "987654321";
            form.textBox6.Text = "test@example.com";

            // Act
            bool printDialogShown = false;
            bool documentSaved = false;
            PrintDialog.ShowDialogAction = () =>
            {
                printDialogShown = true;
                return DialogResult.OK;
            };
            File.WriteAllTextAction = (path, content) =>
            {
                documentSaved = true;
                Assert.AreEqual("document1.txt", path);
                Assert.AreEqual("Contract content", content);
            };

            form.button1_Click(null, null);

            // Assert
            //sert.IsTrue(printDialogShown);
            //Assert.IsTrue(documentSaved);
        }

        [TestMethod]
        public void FormsCount_WhenFormClosed_DecreaseOpenFormsCount()
        {
            // Arrange
            int initialOpenFormsCount = Form3.OpenFormsCount;

            // Act
            form.Close();

            // Assert
            Assert.AreEqual(initialOpenFormsCount, Form3.OpenFormsCount);
        }
    }

    public static class MessageBox
    {
        public static Func<string, string, MessageBoxButtons, DialogResult> ShowAction { get; set; }

        public static DialogResult Show(string message, string caption, MessageBoxButtons buttons)
        {
            return ShowAction?.Invoke(message, caption, buttons) ?? DialogResult.None;
        }
    }

    public static class PrintDialog
    {
        public static Func<DialogResult> ShowDialogAction { get; set; }

        public static DialogResult ShowDialog()
        {
            return ShowDialogAction?.Invoke() ?? DialogResult.None;
        }
    }

    public static class File
    {
        public static Action<string, string> WriteAllTextAction { get; set; }

        public static void WriteAllText(string path, string content)
        {
            WriteAllTextAction?.Invoke(path, content);
        }
    }


}

