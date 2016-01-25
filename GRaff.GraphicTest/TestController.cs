using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.GraphicTest
{
	class TestController : GameElement, IKeyPressListener
	{
		int _testIndex;
		Type[] _tests;
		GameElement _currentTest;

		public TestController()
		{
			var types = Assembly.GetExecutingAssembly()
				.GetTypes();
            _tests = types
				.Where(t => t.GetCustomAttribute<TestAttribute>() != null)
				.ToArray();

			_testIndex = 0;
			_initiateTest(_tests[0]);
		}

		private void _initiateTest(Type test)
		{
			if (_currentTest != null)
				_currentTest.Destroy();

			_currentTest = (GameElement)Activator.CreateInstance(test);
			Instance.Create(_currentTest);
			Window.Title = test.Name;
		}

		public void OnKeyPress(Key key)
		{
			if (key == Key.Right)
				_testIndex = (_testIndex + 1) % _tests.Length;
			else if (key == Key.Left)
				_testIndex = (_testIndex - 1 + _tests.Length) % _tests.Length;
			else return;
			_initiateTest(_tests[_testIndex]);
		}
	}
}
