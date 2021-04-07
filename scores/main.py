import numpy as np
import pandas

N = 20


def main():
    save_df = pandas.read_excel("data.xlsx", sheet_name="Save")
    shoot_df = pandas.read_excel("data.xlsx", sheet_name="Shoot")
    teams = np.array(shoot_df["Squad"])
    shoot = np.array(shoot_df["Shoot%"])
    save = np.array(save_df["Save%"]) / 100

    pass_df = pandas.read_excel("data.xlsx", sheet_name="Pass")
    intercept_df = pandas.read_excel("data.xlsx", sheet_name="Pass (def)")
    s_pass = np.array(pass_df["Short Cmp%"]) / 100
    s_intercept = np.array(intercept_df["Short Cmp%"]) / 100
    l_pass = np.array(pass_df["Long Cmp%"]) / 100
    l_intercept = np.array(intercept_df["Long Cmp%"]) / 100

    dribble_df = pandas.read_excel("data.xlsx", sheet_name="Dribble")
    tackle_df = pandas.read_excel("data.xlsx", sheet_name="Tackle")
    dribble = np.array(dribble_df["Dribble%"]) / 100
    tackle = np.array(tackle_df["Tackle%"]) / 100

    shoot_scores, save_scores, shoot_k, shoot_x0, shoot_diff, shoot_predicted = calc_scores_with_error(shoot, save)
    s_pass_scores, s_intercept_scores, s_pass_k, s_pass_x0, s_pass_diff, s_pass_predicted = calc_scores_with_error(
        s_pass, s_intercept)
    l_pass_scores, l_intercept_scores, l_pass_k, l_pass_x0, l_pass_diff, l_pass_predicted = calc_scores_with_error(
        l_pass, l_intercept)
    dribble_scores, tackle_scores, dribble_k, dribble_x0, dribble_diff, dribble_predicted = calc_scores_with_error(
        dribble, tackle)

    print(np.linalg.norm(shoot_diff))
    print(np.linalg.norm(s_pass_diff))
    print(np.linalg.norm(l_pass_diff))
    print(np.linalg.norm(dribble_diff))

    with open("results.txt", 'w') as f:
        f.write(",".join(teams) + "\n")
        f.write(fmt("shoot", shoot_scores))
        f.write(fmtkx("shoot", shoot_k, shoot_x0))
        f.write(fmt("save", save_scores))
        f.write(fmt("s_pass", s_pass_scores))
        f.write(fmt("l_pass", l_pass_scores))
        f.write(fmt("s_intercept", s_intercept_scores))
        f.write(fmt("l_intercept", l_intercept_scores))
        f.write(fmtkx("s_pass", s_pass_k, s_pass_x0))
        f.write(fmtkx("l_pass", l_pass_k, l_pass_x0))
        f.write(fmt("dribble", dribble_scores))
        f.write(fmt("tackle", tackle_scores))
        f.write(fmtkx("dribble", dribble_k, dribble_x0))
    # print(np.hstack([predicted[:, np.newaxis], dribble[:, np.newaxis]]))
    #
    # print(np.linalg.norm(diff))


def fmtkx(name, k, x):
    return "{}_k = {:.3f}\n{}_x0 = {:.3f}\n".format(name, k, name, x)


def fmt(name, arr):
    return name + " = [" + ", ".join(["{:.0f}".format(i) for i in list(arr)]) + "]\n"


def calc_scores_with_error(attack, defend):
    attack_scores = calc_scores(attack)
    defend_scores = calc_scores(defend)

    # x = attack_scores / defend_scores
    x = np.zeros(N)
    for i in range(N):
        s = 0
        for j in range(N):
            if i == j:
                continue
            s += attack_scores[i] / defend_scores[j]
        x[i] = s / (N - 1)

    y = np.log((1 - attack) / attack)
    # print(shoot)

    k, x0 = calc_kx(x, y)

    # print(k, x0)

    diff, predicted = calc_reprojection_error(attack_scores, defend_scores, k, x0, attack)
    return attack_scores, defend_scores, k, x0, diff, predicted


def calc_reprojection_error(attack_score, def_score, k, x0, y):
    """
    Using the calculated scores and k, x0 values, calculate predicted y^ and compare against y
    :param attack_score:
    :param def_score:
    :param k:
    :param x0:
    :param y:
    :return: y^ - y, y^
    """
    predicted = np.zeros(N)
    for i in range(N):
        s = 0
        for j in range(N):
            if i == j:
                continue
            s += L(attack_score[i] / def_score[j], k, x0)
        predicted[i] = s / (N - 1)
    # predicted = L(shoot_scores / save_scores, k, x0)
    return predicted - y, predicted


def calc_kx(x, y):
    """
    Given a set of score ratios, and the ground truth %, calculate the required k and x0
    :param x: Ratio of attack/defend
    :param y: Ground truth %, e.g. goals/shot in percentage
    :return: k, x0
    """
    A = np.hstack([x.reshape(N, 1), y.reshape(N, 1), np.ones((N, 1))])

    # print(A)

    u, s, vt = np.linalg.svd(A)
    v = vt[-1] / vt[-1, 0]
    # print(s, vt)
    # print("V", v)
    k = 1 / v[1]
    x0 = -v[2]
    return k, x0


def calc_scores(data):
    """
    Assume normal distribution with mean 100 and std 10, calculate and assign each team a score value based on their performance
    :param data:
    :return:
    """
    mean = np.mean(data)
    std = np.std(data)

    # print(mean, std)

    norm = (data - mean) / std

    scores = norm * 10 + 100
    return scores


def L(x, k, x0):
    return 1 / (1 + np.exp(-k * (x - x0)))


if __name__ == '__main__':
    main()
