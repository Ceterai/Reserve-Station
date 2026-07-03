// SPDX-FileCopyrightText: 2026 Space Station 14 Contributors
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.GameStates;

namespace Content.Shared.Overlays;

/// <summary>
/// Makes the entity see air temperature.
/// When added to a clothing item it will also grant the wearer the same overlay.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class ThermalSightComponent : Component;
