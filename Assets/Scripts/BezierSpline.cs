using UnityEngine;
using System;

public class BezierSpline : MonoBehaviour {

	[SerializeField]
	private Vector3[] points;

	[SerializeField]
	private BezierControlPointMode[] modes;

	[SerializeField]
	private bool loop;

    [SerializeField]
    private bool drawGizmos;

	public bool Loop {
		get {
			return loop;
		}
		set {
			loop = value;
			if (value == true) {
				modes[modes.Length - 1] = modes[0];
				SetControlPoint(0, points[0]);
			}
		}
	}
    public bool DrawGizmos {
        get
        {
            return drawGizmos;
        }
        set
        {
            drawGizmos = value;
        }
    }


    public int ControlPointCount {
		get {
			return points.Length;
		}
	}

	public Vector3 GetControlPoint (int index) {
		return points[index];
	}

	public void SetControlPoint (int index, Vector3 point) {
		if (index % 3 == 0) {
			Vector3 delta = point - points[index];
			if (loop) {
				if (index == 0) {
					points[1] += delta;
					points[points.Length - 2] += delta;
					points[points.Length - 1] = point;
				}
				else if (index == points.Length - 1) {
					points[0] = point;
					points[1] += delta;
					points[index - 1] += delta;
				}
				else {
					points[index - 1] += delta;
					points[index + 1] += delta;
				}
			}
			else {
				if (index > 0) {
					points[index - 1] += delta;
				}
				if (index + 1 < points.Length) {
					points[index + 1] += delta;
				}
			}
		}
		points[index] = point;
		EnforceMode(index);
	}

	public BezierControlPointMode GetControlPointMode (int index) {
		return modes[(index + 1) / 3];
	}

	public void SetControlPointMode (int index, BezierControlPointMode mode) {
		int modeIndex = (index + 1) / 3;
		modes[modeIndex] = mode;
		if (loop) {
			if (modeIndex == 0) {
				modes[modes.Length - 1] = mode;
			}
			else if (modeIndex == modes.Length - 1) {
				modes[0] = mode;
			}
		}
		EnforceMode(index);
	}

	private void EnforceMode (int index) {
		int modeIndex = (index + 1) / 3;
		BezierControlPointMode mode = modes[modeIndex];
		if (mode == BezierControlPointMode.Free || !loop && (modeIndex == 0 || modeIndex == modes.Length - 1)) {
			return;
		}

		int middleIndex = modeIndex * 3;
		int fixedIndex, enforcedIndex;
		if (index <= middleIndex) {
			fixedIndex = middleIndex - 1;
			if (fixedIndex < 0) {
				fixedIndex = points.Length - 2;
			}
			enforcedIndex = middleIndex + 1;
			if (enforcedIndex >= points.Length) {
				enforcedIndex = 1;
			}
		}
		else {
			fixedIndex = middleIndex + 1;
			if (fixedIndex >= points.Length) {
				fixedIndex = 1;
			}
			enforcedIndex = middleIndex - 1;
			if (enforcedIndex < 0) {
				enforcedIndex = points.Length - 2;
			}
		}

		Vector3 middle = points[middleIndex];
		Vector3 enforcedTangent = middle - points[fixedIndex];
		if (mode == BezierControlPointMode.Aligned) {
			enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, points[enforcedIndex]);
		}
		points[enforcedIndex] = middle + enforcedTangent;
	}

	public int CurveCount {
		get {
			return (points.Length - 1) / 3;
		}
	}



    public Vector3 GetPoint (float t) {
		int i;
		if (t >= 1f) {
			t = 1f;
			i = points.Length - 4;
		}
		else {
			t = Mathf.Clamp01(t) * CurveCount;
			i = (int)t;
			t -= i;
			i *= 3;
		}
		return transform.TransformPoint(Bezier.GetPoint(points[i], points[i + 1], points[i + 2], points[i + 3], t));
	}
	
	public Vector3 GetVelocity (float t) {
		int i;
		if (t >= 1f) {
			t = 1f;
			i = points.Length - 4;
		}
		else {
			t = Mathf.Clamp01(t) * CurveCount;
			i = (int)t;
			t -= i;
			i *= 3;
		}
		return transform.TransformPoint(Bezier.GetFirstDerivative(points[i], points[i + 1], points[i + 2], points[i + 3], t)) - transform.position;
	}
	
	public Vector3 GetDirection (float t) {
		return GetVelocity(t).normalized;
	}

	public void AddCurve () {
		Vector3 point = points[points.Length - 1];
		Array.Resize(ref points, points.Length + 3);
		point.x += 1f;
		points[points.Length - 3] = point;
		point.x += 1f;
		points[points.Length - 2] = point;
		point.x += 1f;
		points[points.Length - 1] = point;

		Array.Resize(ref modes, modes.Length + 1);
		modes[modes.Length - 1] = modes[modes.Length - 2];
		EnforceMode(points.Length - 4);

		if (loop) {
			points[points.Length - 1] = points[0];
			modes[modes.Length - 1] = modes[0];
			EnforceMode(0);
		}
	}
	
	public void Reset () {
		points = new Vector3[] {
			new Vector3(1f, 0f, 0f),
			new Vector3(2f, 0f, 0f),
			new Vector3(3f, 0f, 0f),
			new Vector3(4f, 0f, 0f)
		};
		modes = new BezierControlPointMode[] {
			BezierControlPointMode.Free,
			BezierControlPointMode.Free
		};
	}

    public Vector3 Evaluate(float aTime)
    {
        var t = Mathf.Clamp01(aTime);
        return (((-points[0] + 3 * (points[1] - points[2]) + points[3]) * t + (3 * (points[0] + points[2]) - 6 * points[1])) * t + 3 * (points[1] - points[0])) * t + points[0];
    }

    // Calculates the best fitting time in the given interval
    private float CPOB(Vector3 aP, float aStart, float aEnd, int aSteps)
    {
        aStart = Mathf.Clamp01(aStart);
        aEnd = Mathf.Clamp01(aEnd);
        float step = (aEnd - aStart) / (float)aSteps;
        float Res = 0;
        float Ref = float.MaxValue;
        for (int i = 0; i < aSteps; i++)
        {
            float t = aStart + step * i;
            float L = (Evaluate(t) - aP).sqrMagnitude;
            if (L < Ref)
            {
                Ref = L;
                Res = t;
            }
        }
        return Res;
    }

    public float ClosestTimeOnBezier(Vector3 aP)
    {
        float t = CPOB(aP, 0, 1, 10);
        float delta = 1.0f / 10.0f;
        for (int i = 0; i < 4; i++)
        {
            t = CPOB(aP, t - delta, t + delta, 10);
            delta /= 9;//10;
        }
        return t;
    }

    public Vector3 ClosestPointOnBezier(Vector3 aP)
    {
        return Evaluate(ClosestTimeOnBezier(aP));
    }

    public float CalculateSplineLength()
    {
        int steps = 1;
        float stepLength = 0.01f;
        float splineLengthInternal = 0;
        for (float i = 0; i < steps; i += stepLength)
        {
            splineLengthInternal += Vector3.Distance(GetPoint(i), GetPoint((i + stepLength)));
        }
        return splineLengthInternal;
    }

    public Vector3 GetNearestPoint(Vector3 pos, float stepLength)
    {
        int steps = 1;
        Vector3 currentClosestVector = Vector3.zero;
        float currentSmallestDist = 5;
        for (float i = 0; i < steps; i += stepLength)
        {
            Vector3 point = GetPoint(i);
            if ((point - pos).y < 5 && (point - pos).y > -5)
            {
                
                point = new Vector3(point.x, pos.y,point.z);
                if (Vector3.Distance(pos, GetPoint(i)) < currentSmallestDist)
                {
                    currentSmallestDist = Vector3.Distance(pos, GetPoint(i));
                    currentClosestVector = GetPoint(i);
                    if (Vector3.Distance(pos, GetPoint(i)) > currentSmallestDist)
                    {
                        break;
                    }
                }
            }
        }
        return currentClosestVector;
    }
    public Vector3 GetNearestDirection(Vector3 pos, float stepLength)
    {
        int steps = 1;
        Vector3 currentClosestVector = Vector3.zero;
        Vector3 closestDirection = Vector3.zero;
        float currentSmallestDist = 5;
        for (float i = 0; i < steps; i += stepLength)
        {
            Vector3 point = GetPoint(i);
            if ((point - pos).y < 5 && (point - pos).y > -5)
            {

                point = new Vector3(point.x, pos.y, point.z);
                if (Vector3.Distance(pos, GetPoint(i)) < currentSmallestDist)
                {
                    currentSmallestDist = Vector3.Distance(pos, GetPoint(i));
                    currentClosestVector = GetPoint(i);
                    closestDirection = GetDirection(i);
                    if (Vector3.Distance(pos, GetPoint(i)) > currentSmallestDist)
                    {
                        break;
                    }
                }
            }
        }
        return closestDirection;
    }

    public float pathWidth;
    public float pathLengthModifier;
    public Color pathColour = new Color(1, 0, 0, 1);
    void OnDrawGizmos()
    {
            Vector3 currentClosestVector = Vector3.zero;
            float stepLength = 0.001f;
            float fillAmount = 0.5f;
            for (float i = 0; i < 1* pathLengthModifier; i += stepLength)
            {
            
                Gizmos.color = pathColour;
                Vector3 point1 = GetPoint(i);
                //Gizmos.DrawWireSphere(point1, pathWidth);
                Vector3 point2 = GetPoint(i + stepLength);
                Vector3 dir1 = point1 - GetPoint(i + stepLength);
                dir1.Normalize();
                dir1 = new Vector3(dir1.x, dir1.y, dir1.z);
                Gizmos.color = Color.white;
                Gizmos.DrawLine(point1, point2);
                dir1 = GetDirection(i);
                Gizmos.color = Color.red;
                Vector3 dir2 = GetDirection(i + stepLength);
                Quaternion perpDir = Quaternion.Euler(new Vector3(90, 0, 0));
                dir1 = (perpDir * dir1).normalized;
                dir2 = (perpDir * dir2).normalized;
                point1 += dir1 * pathWidth * fillAmount;
                point2 += dir2 * pathWidth * fillAmount;
                Gizmos.DrawLine(point1, point2);
                point1 -= dir1 * pathWidth;
                point2 -= dir2 * pathWidth;
                Gizmos.DrawLine(point1, point2);
               
            }
    }
}