// SPDX-FileCopyrightText: 2026 Ceterai <ceterai@protonmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Server._Reserve.Chemistry.Components;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Chemistry;
using Content.Shared.Chemistry.Components.SolutionManager;

namespace Content.Server._Reserve.Chemistry.EntitySystems;

public sealed class SolutionHeaterAutoSystem : EntitySystem
{
    [Dependency] private readonly SharedSolutionContainerSystem _solutionContainer = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SolutionHeaterAutoComponent, ComponentInit>(OnInit);
    }

    private void OnInit(Entity<SolutionHeaterAutoComponent> entity, ref ComponentInit args)
    {
        EnsureComp<ActiveSolutionHeaterAutoComponent>(entity);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<ActiveSolutionHeaterAutoComponent, SolutionHeaterAutoComponent, SolutionContainerManagerComponent>();
        while (query.MoveNext(out var uid, out _, out var heater, out var container))
        {
            if (string.IsNullOrEmpty(heater.Solution))
                continue;

            if (!_solutionContainer.TryGetSolution((uid, container), heater.Solution, out var soln))
                continue;

            var solution = soln.Value.Comp.Solution;
            if (solution.Temperature >= heater.MaxHeat)
                continue;

            var energy = heater.HeatPerSecond * frameTime;
            _solutionContainer.AddThermalEnergy(soln.Value, energy);
        }
    }
}
