using Admin.Web.Controllers;
using Management.Application.Employee.Dtos;
using Management.Application.Employee.Service;
using Management.Domain.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Web.Controllers;

[Authorize(Roles = SystemRoles.Administrator)]
public class EmployeesController : Controller
{
	private readonly IEmployeeService _employeeService;

	public EmployeesController(IEmployeeService employeeService)
	{
		_employeeService = employeeService;
	}

	public async Task<IActionResult> Index(CancellationToken cancellationToken)
	{
		var employees = await _employeeService.GetAllAsync(cancellationToken);
		return View(employees);
	}

	public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
	{
		var employee = await _employeeService.GetByIdAsync(id, cancellationToken);
		return employee is null ? NotFound() : View(employee);
	}

	public IActionResult Create()
	{
		return View(new EmployeeCreateDto());
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(EmployeeCreateDto model, CancellationToken cancellationToken)
	{
		if (!ModelState.IsValid)
		{
			return View(model);
		}

		var (created, error, _) = await _employeeService.CreateAsync(model, cancellationToken);
		if (!created)
		{
			ModelState.AddModelError(string.Empty, error ?? "No se pudo crear el empleado.");
			return View(model);
		}

		return RedirectToAction(nameof(Index));
	}

	public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
	{
		var employee = await _employeeService.GetByIdAsync(id, cancellationToken);
		if (employee is null)
		{
			return NotFound();
		}

		ViewBag.EmployeeId = id;
		return View(MapToUpdateDto(employee));
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(Guid id, EmployeeUpdateDto model, CancellationToken cancellationToken)
	{
		if (!ModelState.IsValid)
		{
			ViewBag.EmployeeId = id;
			return View(model);
		}

		var (updated, error, _) = await _employeeService.UpdateAsync(id, model, cancellationToken);
		if (!updated)
		{
			if (string.Equals(error, "Empleado no encontrado.", StringComparison.OrdinalIgnoreCase))
			{
				return NotFound();
			}

			ModelState.AddModelError(string.Empty, error ?? "No se pudo actualizar el empleado.");
			ViewBag.EmployeeId = id;
			return View(model);
		}

		return RedirectToAction(nameof(Index));
	}

	public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
	{
		var employee = await _employeeService.GetByIdAsync(id, cancellationToken);
		return employee is null ? NotFound() : View(employee);
	}

	[HttpPost, ActionName("Delete")]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
	{
		var deleted = await _employeeService.DeleteAsync(id, cancellationToken);
		return deleted ? RedirectToAction(nameof(Index)) : NotFound();
	}

	private static EmployeeUpdateDto MapToUpdateDto(EmployeeReadDto employee) => new()
	{
		FirstName = employee.FirstName,
		LastName = employee.LastName,
		BirthDate = employee.BirthDate,
		Address = employee.Address,
		Phone = employee.Phone,
		Email = employee.Email,
		Position = employee.Position,
		Salary = employee.Salary,
		HireDate = employee.HireDate,
		State = employee.State,
		EducationLevel = employee.EducationLevel,
		ProfessionalProfile = employee.ProfessionalProfile,
		DocumentNumber = employee.DocumentNumber,
		Department = employee.Department
	};
}
